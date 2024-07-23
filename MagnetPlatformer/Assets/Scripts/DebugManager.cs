using UnityEngine;

public class DebugManager : MonoBehaviour
{
    [Header("Magnetic Interaction Gizmos")]
    public bool NetForce;
    public bool EmissionRadius;

    void Update()
    {
        // Magnetic Interaction Gizmos
        MagneticInteractionGizmos[] magneticInteractionGizmos = FindObjectsOfType<MagneticInteractionGizmos>();
        if (NetForce)
        {
            foreach (MagneticInteractionGizmos gizmos in magneticInteractionGizmos)
            {
                if (gizmos.NetForce == GizmosOverride.Type.Off)
                    gizmos.DrawEmissionRadius();
            }
        }
        if (EmissionRadius)
        {
            foreach (MagneticInteractionGizmos gizmos in magneticInteractionGizmos)
            {
                if (gizmos.EmissionRadius == GizmosOverride.Type.Off)
                    gizmos.DrawEmissionRadius();
            }
        }
    }
}