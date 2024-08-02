using UnityEngine;

public class GizmosSettings : MonoBehaviour
{
    public struct MagneticInteractionSettings
    {
        public bool AppliedForces;
        public bool NetAppliedForce;
        public bool EmissionRadius;
    }

    [Header("Magnetic Interaction")]
    [SerializeField] bool _appliedForcesGizmos;
    [SerializeField] bool _netAppliedForceGizmos;
    [SerializeField] bool _emissionRadiusGizmos;
    public static MagneticInteractionSettings MagneticInteraction;

    public struct EnemySettings
    {
        public bool TargetPoint;
    }

    [Header("Enemy")]
    [SerializeField] bool _targetPoint;
    public static EnemySettings Enemy;

    void OnValidate()
    {
        MagneticInteraction.AppliedForces = _appliedForcesGizmos;
        MagneticInteraction.NetAppliedForce = _netAppliedForceGizmos;
        MagneticInteraction.EmissionRadius = _emissionRadiusGizmos;

        Enemy.TargetPoint = _targetPoint;
    }

    void Reset()
    {
        _appliedForcesGizmos = MagneticInteraction.AppliedForces;
        _netAppliedForceGizmos = MagneticInteraction.NetAppliedForce;
        _emissionRadiusGizmos = MagneticInteraction.EmissionRadius;

        _targetPoint = Enemy.TargetPoint;
    }
}