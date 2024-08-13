using UnityEngine;

public class GizmosSettings
{
    [SerializeField] static GizmosSettingsSO _gizmosSettingsSO;

    public static MagneticInteractionSettings MagneticInteraction;
    public struct MagneticInteractionSettings
    {
        public bool AppliedForces;
        public bool NetAppliedForce;
        public bool EmissionRadius;
    }

    public static EnemySettings Enemy;
    public struct EnemySettings
    {
        public bool TargetPoint;
    }
}