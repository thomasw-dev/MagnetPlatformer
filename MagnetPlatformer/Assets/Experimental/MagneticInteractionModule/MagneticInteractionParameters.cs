using UnityEngine;

[CreateAssetMenu(fileName = "Magnetic Interaction Parameters", order = Constants.MAGNETIC_INTERACTION_PARAMETERS)]
public class MagneticInteractionParameters : ScriptableObject
{
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
}