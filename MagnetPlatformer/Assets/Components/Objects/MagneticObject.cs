using System;
using System.Collections.Generic;
using UnityEngine;

public class MagneticObject : MonoBehaviour
{
    Magnet.Charge _currentCharge;
    public Magnet.Charge CurrentCharge
    {
        get { return _currentCharge;  }
        set
        {
            if (_currentCharge != value)
            {
                OnCurrentChargeChanged?.Invoke(value);
            }
            _currentCharge = value;
        }
    }
    [SerializeField] Magnet.Charge _charge;

    [SerializeField] LayerMask _layerMask = default;

    [Range(0f, 50f)]
    [SerializeField] float _magneticRadius = 1f;
    [Range(0f, 50f)]
    [SerializeField] float _forceFactor = 1f;

    public event Action<Magnet.Charge> OnCurrentChargeChanged;

    Rigidbody2D _rigidbody;

    void Awake()
    {
        _rigidbody = GetComponent<Rigidbody2D>();
    }

    void FixedUpdate()
    {
        if (_currentCharge == Magnet.Charge.Positive || _currentCharge == Magnet.Charge.Negative)
        {
            MagneticEffect();
        }
    }

    void OnValidate()
    {
        CurrentCharge = _charge;
    }

    public void SetCharge(Magnet.Charge charge)
    {
        _currentCharge = charge;
        OnCurrentChargeChanged?.Invoke(charge);
    }

    void MagneticEffect()
    {
        List<Rigidbody2D> magneticEffectors = GetAllMagneticEffectorsWithinDiameter(transform.position, _magneticRadius * 2);
        List<Vector2> forces = new List<Vector2>();
        foreach (var magneticEffector in magneticEffectors)
        {
            Vector2 force = CalculateMagneticForce(_rigidbody, magneticEffector);
            forces.Add(force);
        }
        for (int i = 0; i < forces.Count; i++)
        {
            _rigidbody.AddForce(forces[i] * _forceFactor);
        }
    }

    List<Rigidbody2D> GetAllMagneticEffectorsWithinDiameter(Vector2 position, float diameter)
    {
        List<Rigidbody2D> result = new List<Rigidbody2D>();
        Collider2D[] cols = Physics2D.OverlapCircleAll(position, diameter, _layerMask);
        foreach (var col in cols)
        {
            // If the collider's GameObject is not itself and has MagneticObject component
            if (col.gameObject != this && col.gameObject.TryGetComponent(out MagneticObject magneticObject))
            {
                result.Add(col.gameObject.GetComponent<Rigidbody2D>());
            }
        }
        return result;
    }

    Vector2 CalculateMagneticForce(Rigidbody2D receiverRigidbody, Rigidbody2D effectorRigidbody)
    {
        Vector2 distance = receiverRigidbody.position - effectorRigidbody.position;
        Magnet.Charge receiverCharge = receiverRigidbody.gameObject.GetComponent<MagneticObject>().CurrentCharge;
        Magnet.Charge effectorCharge = effectorRigidbody.gameObject.GetComponent<MagneticObject>().CurrentCharge;

        // Repel or Attract
        int netFactor = 1; // 1 = repel, -1 - attract
        if (receiverCharge == Magnet.Charge.Positive && effectorCharge == Magnet.Charge.Positive) netFactor = 1;
        if (receiverCharge == Magnet.Charge.Positive && effectorCharge == Magnet.Charge.Negative) netFactor = -1;
        if (receiverCharge == Magnet.Charge.Negative && effectorCharge == Magnet.Charge.Positive) netFactor = -1;
        if (receiverCharge == Magnet.Charge.Negative && effectorCharge == Magnet.Charge.Negative) netFactor = 1;

        return distance * netFactor;
    }

    void OnDrawGizmos()
    {
        switch (_currentCharge)
        {
            case Magnet.Charge.Neutral:
                return;
            case Magnet.Charge.Positive:
                Gizmos.color = Color.red;
                break;
            case Magnet.Charge.Negative:
                Gizmos.color = Color.blue;
                break;
        }
        Gizmos.DrawWireSphere(transform.position, _magneticRadius);
    }
}
