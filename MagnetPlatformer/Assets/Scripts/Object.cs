using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Object : MonoBehaviour
{
    Rigidbody2D _rigidbody;

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
            if (Magnet.CurrentCharge == Magnet.Charges.Positive) chargeFactor = -1f;
            if (Magnet.CurrentCharge == Magnet.Charges.Negative) chargeFactor = 1f;

            _rigidbody.AddForce((GameObject.Find("Player").transform.position - transform.position) * chargeFactor);
        }
    }

    void AddForceTowards(Vector3 direction, float magnitude)
    {

    }
}
