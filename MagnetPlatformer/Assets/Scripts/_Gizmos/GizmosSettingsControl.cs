using UnityEngine;

[CreateAssetMenu(fileName = "Current Settings", order = Constants.GIZMOS_SETTINGS_CONTROL)]
public class GizmosSettingsControl : ScriptableObject
{
    [Header("Magnetic Interaction")]
    [SerializeField] bool _appliedForces;
    [SerializeField] bool _netAppliedForce;
    [SerializeField] bool _emissionRadius;

    [Header("Enemy")]
    [SerializeField] bool _targetPoint;
    [SerializeField] bool _dashEnableDistance;

    void OnValidate()
    {
        GizmosSettings.MagneticInteraction.AppliedForces = _appliedForces;
        GizmosSettings.MagneticInteraction.NetAppliedForce = _netAppliedForce;
        GizmosSettings.MagneticInteraction.EmissionRadius = _emissionRadius;

        GizmosSettings.Enemy.TargetPoint = _targetPoint;
        GizmosSettings.Enemy.DashEnableDistance = _dashEnableDistance;
    }
}
