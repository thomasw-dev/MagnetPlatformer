using System;
using System.Collections.Generic;
using UnityEngine;

public class MagneticObject : MonoBehaviour
{
    [Range(0f, 100f)]
    public float Force = 1f;

    [Range(0f, 50f)]
    public float Radius = 5f;

    [Tooltip("If the charge is constant, player cannot change the charge of this object.")]
    [SerializeField] bool _constantCharge;
    bool _chargeIsConstant;
    [HideInInspector]
    public bool ChargeIsConstant
    {
        get { return _chargeIsConstant; }
        set
        {
            if (_chargeIsConstant != value) { OnChargeIsConstantChanged?.Invoke(value); }
            _chargeIsConstant = value;
        }
    }

    [Space(10)]

    [SerializeField] bool _neutral = true;
    [SerializeField] bool _positive = false;
    [SerializeField] bool _negative = false;

    Magnet.Charge _currentCharge;
    public Magnet.Charge CurrentCharge
    {
        get { return _currentCharge; }
        set
        {
            if (_currentCharge != value) { OnCurrentChargeChanged?.Invoke(value); }
            _currentCharge = value;
        }
    }

    [Space(10)]

    [SerializeField] bool _useGravity = true;
    [SerializeField] LayerMask _layerMask = default;
    [SerializeField] Transform _visualChild;

    [Space(10)]

    [SerializeField] bool gizmos = false;

    public event Action<Magnet.Charge> OnCurrentChargeChanged;
    public event Action<bool> OnChargeIsConstantChanged;

    Rigidbody2D _rigidbody;

    void Awake()
    {
        _rigidbody = GetComponent<Rigidbody2D>();
    }

    void OnEnable()
    {
        OnCurrentChargeChanged += UpdateChargeBools;
        OnCurrentChargeChanged += UpdateVisual;
    }

    void OnDisable()
    {
        OnCurrentChargeChanged -= UpdateChargeBools;
        OnCurrentChargeChanged -= UpdateVisual;
    }

    void FixedUpdate()
    {
        if (_useGravity)
        {
            GravityForce();
        }

        if (CurrentCharge != Magnet.Charge.Neutral)
        {
            MagneticEffect();
        }
    }

    void OnValidate()
    {
        ChargeIsConstant = _constantCharge;

        if (CurrentCharge == Magnet.Charge.Neutral)
        {
            if (_positive && !_negative) CurrentCharge = Magnet.Charge.Positive;
            if (!_positive && _negative) CurrentCharge = Magnet.Charge.Negative;
        }

        else if (CurrentCharge == Magnet.Charge.Positive)
        {
            if (_neutral && !_negative) CurrentCharge = Magnet.Charge.Neutral;
            if (!_neutral && _negative) CurrentCharge = Magnet.Charge.Negative;
        }

        else if (CurrentCharge == Magnet.Charge.Negative)
        {
            if (_neutral && !_positive) CurrentCharge = Magnet.Charge.Neutral;
            if (!_neutral && _positive) CurrentCharge = Magnet.Charge.Positive;
        }

        UpdateChargeBools(CurrentCharge);
        UpdateVisual(CurrentCharge);
    }

    public void SetCharge(Magnet.Charge charge)
    {
        CurrentCharge = charge;
    }

    void MagneticEffect()
    {
        List<Vector2> forces = new List<Vector2>();

        // Identify the magnetic objects in effect
        List<GameObject> magneticObjectsNearby = GetAllNearbyMagneticObjects(transform.position, Radius * 2);
        List<GameObject> magneticObjectsinEffect;
        // If it is assigned a Magnet Object Group, it can only interact with magnetic objects within the group.
        // Otherwise, interact with all other magnetic objects.
        if (TryGetComponent(out AddToMagnetObjectGroup magnetObjectGroup))
        {
            MagnetObjectGroup group = magnetObjectGroup.MagnetObjectGroup;
            if (group != null)
            {
                magneticObjectsinEffect = IncludeMatchedGroupObjects(magneticObjectsNearby, group);
            }
            else magneticObjectsinEffect = magneticObjectsNearby;
        }
        else magneticObjectsinEffect = magneticObjectsNearby;

        // Calculate the forces
        foreach (var magneticObject in magneticObjectsinEffect)
        {
            MagneticObject target = magneticObject.GetComponent<MagneticObject>();
            Vector2 selfTargetDistance = transform.position - magneticObject.transform.position;
            Vector2 force = MagneticForce.Calculate(_rigidbody.velocity, selfTargetDistance, target.Force);
            force = MagneticForce.AdjustForceByCharge(force, CurrentCharge, target.CurrentCharge);
            forces.Add(force);
        }

        // Apply the forces
        for (int i = 0; i < forces.Count; i++)
        {
            _rigidbody.AddForce(forces[i]);
        }
    }

    List<GameObject> GetAllNearbyMagneticObjects(Vector2 position, float diameter)
    {
        List<GameObject> output = new List<GameObject>();
        Collider2D[] cols = Physics2D.OverlapCircleAll(position, diameter, _layerMask);
        foreach (var col in cols)
        {
            output.Add(col.gameObject);
        }
        return output;
    }

    List<GameObject> IncludeMatchedGroupObjects(List<GameObject> magneticObjects, MagnetObjectGroup group)
    {
        List<GameObject> output = new List<GameObject>();
        foreach (var magneticObject in magneticObjects)
        {
            if (magneticObject.TryGetComponent(out AddToMagnetObjectGroup magnetObjectGroup))
            {
                if (magnetObjectGroup.MagnetObjectGroup == group)
                {
                    output.Add(magneticObject);
                }
            }
        }
        return output;
    }

    void GravityForce()
    {
        Vector2 gravityForce = MagneticForce.Calculate(_rigidbody.velocity, Vector2.down, 1f);
        _rigidbody.AddForce(gravityForce);
    }

    void OnDrawGizmosSelected()
    {
        if (gizmos)
        {
            switch (CurrentCharge)
            {
                case Magnet.Charge.Neutral: return;
                case Magnet.Charge.Positive: Gizmos.color = Color.red; break;
                case Magnet.Charge.Negative: Gizmos.color = Color.blue; break;
            }
            Gizmos.DrawWireSphere(transform.position, Radius);
        }

        if (Log.MagneticForceOnSelected)
        {
            Debug.Log($"Net force on this object: {_rigidbody.velocity}");
        }
    }

    #region Inspector

    void UpdateChargeBools(Magnet.Charge charge)
    {
        _neutral = charge == Magnet.Charge.Neutral;
        _positive = charge == Magnet.Charge.Positive;
        _negative = charge == Magnet.Charge.Negative;
    }

    void UpdateVisual(Magnet.Charge charge)
    {
        List<MagneticObjectVisual> magneticObjectVisuals = GetMagneticObjectVisualInChildren(_visualChild);
        foreach (var magneticObjectVisual in magneticObjectVisuals)
        {
            magneticObjectVisual.UpdateSprite(charge);
        }
    }

    List<MagneticObjectVisual> GetMagneticObjectVisualInChildren(Transform parent)
    {
        List<MagneticObjectVisual> output = new List<MagneticObjectVisual>();
        for (int i = 0; i < parent.childCount; i++)
        {
            Transform child = parent.GetChild(i);
            if (child.TryGetComponent(out MagneticObjectVisual magneticObjectVisual))
            {
                output.Add(magneticObjectVisual);
            }
        }
        return output;
    }

    #endregion
}