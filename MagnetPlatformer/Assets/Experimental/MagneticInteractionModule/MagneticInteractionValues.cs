using UnityEngine;

public class MagneticInteractionValues : MonoBehaviour
{
    // These fields are required to be assigned in order for this module to function.
    [Header("Dependencies")]
    public MagneticInteractionConfig Config;

    [Header("Emission")]

    public bool EmitForce = true;

    [Range(0f, 1000f)]
    public float Force = 1f;

    [Range(0f, 50f)]
    public float Radius = 10f;

    [Header("Reaction")]

    public bool ReactToForce = true;

    [Header("Others")]

    public bool UseGravity = true;

    [Header("Gizmos")]

    public bool EmissionRadius = true;

    [ContextMenu("Load Values From Instance")]
    void LoadValuesFromInstance()
    {
        Method.AssignFields(Config, this);
    }

    [ContextMenu("Save Values To Instance")]
    void SaveValuesToInstance()
    {
        Method.AssignFields(this, Config);
    }
}
