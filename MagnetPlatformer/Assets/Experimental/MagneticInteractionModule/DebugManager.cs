using UnityEngine;

public class DebugManager : MonoBehaviour
{
    [Header("Magnetic Interaction")]
    [SerializeField] bool _reactionForcesGizmos;
    [SerializeField] bool _netReactionForceGizmos;
    [SerializeField] bool _emissionRadiusGizmos;
    public static MagneticInteractionGizmosSettings MagneticInteractionGizmosSettings;

    void OnValidate()
    {
        MagneticInteractionGizmosSettings.ReactionForces = _reactionForcesGizmos;
        MagneticInteractionGizmosSettings.NetReactionForce = _netReactionForceGizmos;
        MagneticInteractionGizmosSettings.EmissionRadius = _emissionRadiusGizmos;
    }
}

public struct MagneticInteractionGizmosSettings
{
    public bool ReactionForces;
    public bool NetReactionForce;
    public bool EmissionRadius;
}