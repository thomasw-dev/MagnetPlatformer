using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(MagneticInteractionValues))]
public class MagneticInteractionGizmos : MonoBehaviour
{
    public bool EmissionRadius = true;

    List<bool> gizmosPersistent = new List<bool>();
    List<bool> gizmosSelected = new List<bool>();

    void OnValidate()
    {

    }

    void OnDrawGizmos()
    {

    }

    void OnDrawGizmosSelected()
    {
        DrawEmissionRadius();
    }

    MagneticInteractionConfig GetConfig()
    {
        return GetComponent<MagneticInteractionConfig>();
    }

    void DrawEmissionRadius()
    {
        if (EmissionRadius)
        {
            Gizmos.DrawWireSphere(transform.position, GetConfig().Radius);
        }
    }
}
