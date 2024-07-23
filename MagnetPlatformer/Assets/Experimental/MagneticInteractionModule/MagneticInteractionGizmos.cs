using UnityEngine;

[RequireComponent(typeof(MagneticInteractionValues))]
public class MagneticInteractionGizmos : MonoBehaviour
{
    [Header("Gizmos Overrides")]
    public GizmosOverride.Type NetForce;
    public GizmosOverride.Type EmissionRadius;

    void OnDrawGizmosSelected()
    {
        if (EmissionRadius == GizmosOverride.Type.OnSelected) DrawNetForce();
        if (EmissionRadius == GizmosOverride.Type.OnSelected) DrawEmissionRadius();
    }

    void OnDrawGizmos()
    {
        if (EmissionRadius == GizmosOverride.Type.Persistent) DrawNetForce();
        if (EmissionRadius == GizmosOverride.Type.Persistent) DrawEmissionRadius();
    }

    MagneticInteractionValues GetValue()
    {
        return GetComponent<MagneticInteractionValues>();
    }

    public void DrawNetForce()
    {
        const float LENGTH_MAX = 5f;
        float length = Method.Map(GetValue().Force, 0, MagneticForce.MAX_FORCE, 0, LENGTH_MAX);
        Gizmos.color = Color.cyan;
        Gizmos.DrawWireSphere(transform.position, length);
    }

    public void DrawEmissionRadius()
    {
        Gizmos.color = Color.white;
        Gizmos.DrawWireSphere(transform.position, GetValue().Radius);
    }
}