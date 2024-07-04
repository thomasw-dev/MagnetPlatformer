using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Obsolete
/// </summary>
public class Object : MonoBehaviour
{
    Rigidbody2D _rigidbody;

    [SerializeField] Magnet.Charge _selfCharge;

    void Start()
    {
        _rigidbody = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        // Check distance to see if self is in range of attraction/repulsion
        float distance = Vector2.Distance(transform.position, GameObject.Find("Player").transform.position);
        if (distance < 5f)
        {
            float chargeFactor = 0;
            if (MagnetWeapon.CurrentCharge == Magnet.Charge.Positive) chargeFactor = -1f;
            if (MagnetWeapon.CurrentCharge == Magnet.Charge.Negative) chargeFactor = 1f;

            if (_selfCharge == Magnet.Charge.Negative) chargeFactor = -chargeFactor;
            _rigidbody.AddForce((GameObject.Find("Player").transform.position - transform.position) * chargeFactor);
        }
    }

    void AddForceTowards(Vector3 direction, float magnitude)
    {

    }
}
