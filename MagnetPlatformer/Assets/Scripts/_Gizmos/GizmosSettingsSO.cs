using UnityEngine;

[CreateAssetMenu(fileName = "Gizmos Settings", order = Constants.GIZMOS_SETTINGS_SO)]
public class GizmosSettingsSO : ScriptableObject
{
    [Header("Value Set")]
    [SerializeField] GizmosSettingsSO _gizmosSettingsSO;

    [Header("Magnetic Interaction")]
    [SerializeField] bool _appliedForcesGizmos;
    [SerializeField] bool _netAppliedForceGizmos;
    [SerializeField] bool _emissionRadiusGizmos;

    [Header("Enemy")]
    [SerializeField] bool _targetPoint;
}
