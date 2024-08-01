using UnityEngine;

public class MagneticInteractionGizmos : MonoBehaviour
{
    const float FORCE_MAGNITUDE_FACTOR = 0.002f;
    const float FORCE_LENGTH_MAX = 5f;

    static Color EMISSION_RADIUS_COLOR_NEUTRAL = Color.grey;
    static Color EMISSION_RADIUS_COLOR_POSITIVE = Color.red;
    static Color EMISSION_RADIUS_COLOR_NEGATIVE = Color.blue;
    static Color ATTRACTION_FORCE_COLOR = Color.green;
    static Color REPULSION_FORCE_COLOR = Color.yellow;
    static Color NET_FORCE_COLOR = Color.magenta;

    MagneticInteractionController GetController() => transform.parent.GetComponent<MagneticInteractionController>();

    MagneticInteractionValues GetValues() => transform.parent.GetComponent<MagneticInteractionValues>();

    void OnDrawGizmos()
    {
        if (DebugManager.MagneticInteractionGizmosSettings.AppliedForces)
        {
            for (int i = 0; i < GetController().AppliedForces.Count; i++)
            {
                ChargedForce reactionForce = GetController().AppliedForces[i];

                Gizmos.color = reactionForce.Relation == ChargedForce.RelationType.Attract ? ATTRACTION_FORCE_COLOR : REPULSION_FORCE_COLOR;
                Gizmos.DrawRay(transform.position, Vector2.ClampMagnitude(reactionForce.Vector * FORCE_MAGNITUDE_FACTOR, FORCE_LENGTH_MAX));
            }
        }

        if (DebugManager.MagneticInteractionGizmosSettings.NetAppliedForce)
        {
            if (GetController().AppliedForces.Count <= 1) { return; }

            Vector2 netReactionForce = GetController().NetAppliedForce;
            Gizmos.color = NET_FORCE_COLOR;
            Gizmos.DrawRay(transform.position, Vector2.ClampMagnitude(netReactionForce * FORCE_MAGNITUDE_FACTOR, FORCE_LENGTH_MAX));
        }

        if (DebugManager.MagneticInteractionGizmosSettings.EmissionRadius)
        {
            if (GetController().CurrentCharge == Magnet.Charge.Neutral) Gizmos.color = EMISSION_RADIUS_COLOR_NEUTRAL;
            if (GetController().CurrentCharge == Magnet.Charge.Positive) Gizmos.color = EMISSION_RADIUS_COLOR_POSITIVE;
            if (GetController().CurrentCharge == Magnet.Charge.Negative) Gizmos.color = EMISSION_RADIUS_COLOR_NEGATIVE;
            Gizmos.DrawWireSphere(transform.position, GetValues().EmissionRadius);
        }
    }
}