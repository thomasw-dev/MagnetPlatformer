using UnityEngine;

[CreateAssetMenu(fileName = "Gizmos Settings Control", order = Constants.GIZMOS_SETTINGS_CONTROL)]
public class GizmosSettingsControl : ScriptableObject
{
    [Header("Magnetic Interaction")]
    [SerializeField] bool _appliedForcesGizmos;
    [SerializeField] bool _netAppliedForceGizmos;
    [SerializeField] bool _emissionRadiusGizmos;

    [Header("Enemy")]
    [SerializeField] bool _targetPoint;

    void OnValidate()
    {
        //CurrentSettings = _assignedSet;
        Debug.Log("Updated CurrentSettings");
    }
}
