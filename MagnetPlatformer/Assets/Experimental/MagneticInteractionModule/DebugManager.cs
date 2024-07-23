using UnityEngine;

public class DebugManager : MonoBehaviour
{
    [Header("Magnetic Interaction")]
    [SerializeField] bool _netForceGizmos;
    [SerializeField] bool _emissionRadiusGizmos;
    public static MagneticInteractionGizmosSettings MagneticInteractionGizmosSettings;

    void OnValidate()
    {
        MagneticInteractionGizmosSettings.NetForce = _netForceGizmos;
        MagneticInteractionGizmosSettings.EmissionRadius = _emissionRadiusGizmos;
    }
}

public struct MagneticInteractionGizmosSettings
{
    public bool NetForce;
    public bool EmissionRadius;
}