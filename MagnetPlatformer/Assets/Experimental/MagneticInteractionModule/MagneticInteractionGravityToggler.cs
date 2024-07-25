using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MagneticInteractionGravityToggler : MonoBehaviour
{
    [Header("Dependencies")]
    // These fields are required to be assigned in order for this module to function.

    [SerializeField] Rigidbody2D _rigidbody;
    [SerializeField] MagneticInteractionController _magneticInteractionController;

    bool DependenciesNullCheck()
    {
        bool pass = true;
        if (_rigidbody == null)
        {
            Debug.LogError($"Dependency missing: Rigidbody is not assigned.", this);
            pass = false;
        }
        if (_magneticInteractionController == null)
        {
            Debug.LogError($"Dependency missing: MagneticInteractionController is not assigned.", this);
            pass = false;
        }
        return pass;
    }

    void Awake()
    {
        DependenciesNullCheck();
    }

    void Update()
    {
        bool hasAnyAppliedForce = false;
        foreach (var appliedForce in _magneticInteractionController.AppliedForces)
        {
            if (appliedForce.Relation != ChargedForce.RelationType.Neutral)
            {
                hasAnyAppliedForce = true;
                break;
            }
        }

        if (hasAnyAppliedForce)
        {
            _rigidbody.gravityScale = 0f;
        }
        else
        {
            _rigidbody.gravityScale = 1f;
        }
    }
}
