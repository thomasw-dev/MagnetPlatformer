using System;
using System.Collections.Generic;
using UnityEngine;

public class MagneticObject : MonoBehaviour
{
    [Range(0f, 5f)]
    public float Gain = 1f;

    [Range(0f, 20f)]
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

    void FixedUpdate()
    {
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

        _neutral = CurrentCharge == Magnet.Charge.Neutral;
        _positive = CurrentCharge == Magnet.Charge.Positive;
        _negative = CurrentCharge == Magnet.Charge.Negative;

        UpdateVisual(CurrentCharge);
    }

    public void SetCharge(Magnet.Charge charge)
    {
        CurrentCharge = charge;
    }

    void MagneticEffect()
    {
        List<GameObject> magneticObjectsNearby = GetAllNearbyMagneticObjects(transform.position, _magneticRadius * 2);
        List<GameObject> magneticObjectsinEffect = new List<GameObject>();
        if (TryGetComponent(out AddToMagnetObjectGroup magnetObjectGroup))
        {
            MagnetObjectGroup group = magnetObjectGroup.MagnetObjectGroup;
            if (magnetObjectGroup.MagnetObjectGroup != null)
            {
                magneticObjectsinEffect = DiscardUnmatchedGroupObjects(magneticObjectsNearby, group);
            }
            else magneticObjectsinEffect = magneticObjectsNearby;
        }
        else magneticObjectsinEffect = magneticObjectsNearby;
    }

    List<GameObject> GetAllNearbyMagneticObjects(Vector2 position, float diameter)
    {
        List<GameObject> result = new List<GameObject>();
        Collider2D[] cols = Physics2D.OverlapCircleAll(position, diameter, _layerMask);
        foreach (var col in cols)
        {
            result.Add(col.gameObject);
        }
        return result;
    }

    List<GameObject> DiscardUnmatchedGroupObjects(List<GameObject> magneticObjects, MagnetObjectGroup group)
    {
        List<GameObject> result = new List<GameObject>();
        foreach (var magneticObject in magneticObjects)
        {
            if (TryGetComponent(out AddToMagnetObjectGroup magnetObjectGroup))
            {
                if (true) // !!!
                {
                    result.Add(magneticObject);
                }
            }
            else result.Add(magneticObject);
        }
        return result;
    }

    float _magneticRadius;
    float _forceFactor;

    void Prev_MagneticEffect()
    {
        List<Rigidbody2D> magneticEffectors = Prev_GetAllMagneticObjectRigidbodiesWithinDiameter(transform.position, _magneticRadius * 2);
        List<Vector2> forces = new List<Vector2>();
        foreach (var magneticEffector in magneticEffectors)
        {
            Vector2 forceMagnitude = Prev_CalculateMagneticForceMagnitude(_rigidbody, magneticEffector);
            int chargeFactor = Prev_CalculateChargeFactor(_rigidbody.gameObject.GetComponent<MagneticObject>().CurrentCharge, magneticEffector.gameObject.GetComponent<MagneticObject>().CurrentCharge);
            forces.Add(forceMagnitude * chargeFactor * _forceFactor);
        }
        for (int i = 0; i < forces.Count; i++)
        {
            _rigidbody.AddForce(forces[i] * _forceFactor);
        }
    }

    List<Rigidbody2D> Prev_GetAllMagneticObjectRigidbodiesWithinDiameter(Vector2 position, float diameter)
    {
        List<Rigidbody2D> result = new List<Rigidbody2D>();
        Collider2D[] cols = Physics2D.OverlapCircleAll(position, diameter, _layerMask);

        bool selfIsInGroup = false;
        MagnetObjectGroup selfGroup = null;
        if (gameObject.TryGetComponent(out AddToMagnetObjectGroup selfAddToGroup))
        {
            selfIsInGroup = true;
            selfGroup = selfAddToGroup.MagnetObjectGroup;
        }

        foreach (var col in cols)
        {
            // If the collider's GameObject is not itself and has MagneticObject component
            if (col.gameObject != gameObject && col.gameObject.TryGetComponent(out MagneticObject magneticObject))
            {
                // Only include magnetic objects that are charged
                if (magneticObject.CurrentCharge != Magnet.Charge.Neutral)
                {
                    bool targetIsInGroup = false;
                    MagnetObjectGroup targetGroup = null;
                    if (col.gameObject.TryGetComponent(out AddToMagnetObjectGroup targetAddToGroup))
                    {
                        targetIsInGroup = true;
                        targetGroup = targetAddToGroup.MagnetObjectGroup;
                    }

                    if (selfIsInGroup || targetIsInGroup)
                    {
                        if (selfGroup == targetGroup && selfGroup != null && targetGroup != null)
                        {
                            // Only add if they are in the same group
                            result.Add(col.gameObject.GetComponent<Rigidbody2D>());
                        }
                    }
                    else result.Add(col.gameObject.GetComponent<Rigidbody2D>());
                }
            }
        }
        return result;
    }

    Vector2 Prev_CalculateMagneticForceMagnitude(Rigidbody2D receiverRigidbody, Rigidbody2D effectorRigidbody)
    {
        Vector2 distance = receiverRigidbody.position - effectorRigidbody.position;
        float inverseX = distance.x > 0 ? _magneticRadius * 2 - distance.x : -(_magneticRadius * 2) + distance.x;
        float inverseY = distance.y > 0 ? _magneticRadius * 2 - distance.y : -(_magneticRadius * 2) + distance.y;
        return new Vector2(inverseX, inverseY);
    }

    int Prev_CalculateChargeFactor(Magnet.Charge receiverCharge, Magnet.Charge effectorCharge)
    {
        // Repel or Attract
        int chargeFactor = 1; // 1 = repel, -1 = attract
        if (receiverCharge == Magnet.Charge.Positive && effectorCharge == Magnet.Charge.Positive) chargeFactor = 1;
        if (receiverCharge == Magnet.Charge.Positive && effectorCharge == Magnet.Charge.Negative) chargeFactor = -1;
        if (receiverCharge == Magnet.Charge.Negative && effectorCharge == Magnet.Charge.Positive) chargeFactor = -1;
        if (receiverCharge == Magnet.Charge.Negative && effectorCharge == Magnet.Charge.Negative) chargeFactor = 1;
        return chargeFactor;
    }

    void OnDrawGizmos()
    {
        if (gizmos)
        {
            switch (_currentCharge)
            {
                case Magnet.Charge.Neutral: return;
                case Magnet.Charge.Positive: Gizmos.color = Color.red; break;
                case Magnet.Charge.Negative: Gizmos.color = Color.blue; break;
            }
            Gizmos.DrawWireSphere(transform.position, _magneticRadius);
        }
    }

    #region Inspector

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
        List<MagneticObjectVisual> result = new List<MagneticObjectVisual>();
        for (int i = 0; i < parent.childCount; i++)
        {
            Transform child = parent.GetChild(i);
            if (child.TryGetComponent(out MagneticObjectVisual magneticObjectVisual))
            {
                result.Add(magneticObjectVisual);
            }
        }
        return result;

        #endregion
    }
}