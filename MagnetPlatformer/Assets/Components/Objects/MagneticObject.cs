using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MagneticObject : MonoBehaviour
{
    [SerializeField] Magnet.Charge _currentCharge;
    [SerializeField] LayerMask _layerMask = default;

    [Range(0f, 20f)]
    [SerializeField] float _magneticRadius = 1f;
    [Range(0f, 10f)]
    [SerializeField] float _forceFactor = 1f;

    public event Action<Magnet.Charge> OnCurrentChargeChanged;

    Rigidbody2D _rigidbody;

    void Awake()
    {
        _rigidbody = GetComponent<Rigidbody2D>();
    }

    void Start()
    {

    }

    private void FixedUpdate()
    {
        if (_currentCharge == Magnet.Charge.Positive || _currentCharge == Magnet.Charge.Negative)
        {
            MagneticEffect();
        }
    }

    void MagneticEffect()
    {
        List<Rigidbody2D> magneticEffectors = GetAllMagneticEffectorsWithinRadius(transform.position, _magneticRadius);
        Debug.Log($"magneticEffectors.Count: {magneticEffectors.Count}");
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

    List<Rigidbody2D> GetAllMagneticEffectorsWithinRadius(Vector2 position, float radius)
    {
        List<Rigidbody2D> result = new List<Rigidbody2D>();
        Collider2D[] cols = Physics2D.OverlapCircleAll(position, radius, _layerMask);
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
        return receiverRigidbody.position - effectorRigidbody.position;
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
