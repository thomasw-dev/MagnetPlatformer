using UnityEditor;
using UnityEngine;

public class MagneticInteractionGizmos : MonoBehaviour
{
    [Header("Gizmos Overrides")]
    public GizmosOverride.Type ReactionForces;
    public GizmosOverride.Type NetReactionForce;
    public GizmosOverride.Type EmissionRadius;

    MagneticInteractionGizmosSettings _localSettings;
    public struct MagneticInteractionGizmosSettings
    {
        public GizmosOverride.Type ReactionForces;
        public GizmosOverride.Type NetReactionForce;
        public GizmosOverride.Type EmissionRadius;
    }

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

    void OnValidate()
    {
        _localSettings.ReactionForces = ReactionForces;
        _localSettings.NetReactionForce = NetReactionForce;
        _localSettings.EmissionRadius = EmissionRadius;
    }

    bool SettingEnabled(GizmosOverride.Type local, bool master)
    {
        if (local == GizmosOverride.Type.NoOverride)
        {
            return master;
        }
        else
        {
            if (local == GizmosOverride.Type.Hidden) return false;
            if (local == GizmosOverride.Type.OnSelected) return Selection.activeGameObject == gameObject;
            if (local == GizmosOverride.Type.Persistent) return true;
        }
        return false;
    }

    void OnDrawGizmos()
    {
        if (SettingEnabled(_localSettings.NetReactionForce, DebugManager.MagneticInteractionGizmosSettings.NetReactionForce))
            Draw_NetAppliedForce();

        if (SettingEnabled(_localSettings.ReactionForces, DebugManager.MagneticInteractionGizmosSettings.ReactionForces))
            Draw_AppliedForces();

        if (SettingEnabled(_localSettings.EmissionRadius, DebugManager.MagneticInteractionGizmosSettings.EmissionRadius))
            Draw_EmissionRadius();

        void Draw_AppliedForces()
        {
            for (int i = 0; i < GetController().AppliedForces.Count; i++)
            {
                ChargedForce reactionForce = GetController().AppliedForces[i];

                Gizmos.color = reactionForce.Relation == ChargedForce.RelationType.Attract ? ATTRACTION_FORCE_COLOR : REPULSION_FORCE_COLOR;
                Gizmos.DrawRay(transform.position, Vector2.ClampMagnitude(reactionForce.Vector * FORCE_MAGNITUDE_FACTOR, FORCE_LENGTH_MAX));
            }
        }

        void Draw_NetAppliedForce()
        {
            if (GetController().AppliedForces.Count <= 1) { return; }

            Vector2 netReactionForce = GetController().NetAppliedForce;
            Gizmos.color = NET_FORCE_COLOR;
            Gizmos.DrawRay(transform.position, Vector2.ClampMagnitude(netReactionForce * FORCE_MAGNITUDE_FACTOR, FORCE_LENGTH_MAX));
        }

        void Draw_EmissionRadius()
        {
            if (GetController().CurrentCharge == Magnet.Charge.Neutral) Gizmos.color = EMISSION_RADIUS_COLOR_NEUTRAL;
            if (GetController().CurrentCharge == Magnet.Charge.Positive) Gizmos.color = EMISSION_RADIUS_COLOR_POSITIVE;
            if (GetController().CurrentCharge == Magnet.Charge.Negative) Gizmos.color = EMISSION_RADIUS_COLOR_NEGATIVE;
            Gizmos.DrawWireSphere(transform.position, GetValues().EmissionRadius);
        }
    }
}