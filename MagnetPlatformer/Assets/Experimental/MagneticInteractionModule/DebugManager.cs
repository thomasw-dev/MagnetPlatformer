using UnityEngine;

public class DebugManager : MonoBehaviour
{
    [Header("Magnetic Interaction")]
    [SerializeField] bool _appliedForcesGizmos;
    [SerializeField] bool _netAppliedForceGizmos;
    [SerializeField] bool _emissionRadiusGizmos;
    public static MagneticInteractionGizmosSettings MagneticInteractionGizmosSettings;

    [Header("Enemy")]
    [SerializeField] bool _chaseRadius;
    public static EnemySettings EnemySettings;

    void OnValidate()
    {
        MagneticInteractionGizmosSettings.AppliedForces = _appliedForcesGizmos;
        MagneticInteractionGizmosSettings.NetAppliedForce = _netAppliedForceGizmos;
        MagneticInteractionGizmosSettings.EmissionRadius = _emissionRadiusGizmos;

        EnemySettings.ChaseRadius = _chaseRadius;
    }
}

public struct MagneticInteractionGizmosSettings
{
    public bool AppliedForces;
    public bool NetAppliedForce;
    public bool EmissionRadius;
}

public struct EnemySettings
{
    public bool ChaseRadius;
}