using UnityEditor;
using UnityEngine;

[RequireComponent(typeof(MagneticInteractionValues))]
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

    const float FORCE_LENGTH_MAX = 5f;

    static Color EMISSION_RADIUS_COLOR = Color.white;
    static Color ATTRACTION_FORCE_COLOR = Color.green;
    static Color REPULSION_FORCE_COLOR = Color.yellow;
    static Color NET_FORCE_COLOR = Color.magenta;

    MagneticInteractionController GetController() => GetComponent<MagneticInteractionController>();

    MagneticInteractionValues GetValues() => GetComponent<MagneticInteractionValues>();

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
        if (SettingEnabled(_localSettings.ReactionForces, DebugManager.MagneticInteractionGizmosSettings.ReactionForces))
            DrawReactionForces();

        if (SettingEnabled(_localSettings.NetReactionForce, DebugManager.MagneticInteractionGizmosSettings.NetReactionForce))
            DrawNetReactionForce();

        if (SettingEnabled(_localSettings.EmissionRadius, DebugManager.MagneticInteractionGizmosSettings.EmissionRadius))
            DrawEmissionRadius();

        void DrawReactionForces()
        {
            for (int i = 0; i < GetController().ReactionForces.Count; i++)
            {
                ChargedForce reactionForce = GetController().ReactionForces[i];
                float clampedMagnitude = Method.Map(reactionForce.Vector.magnitude, -MagneticForce.MAX_FORCE, MagneticForce.MAX_FORCE, -FORCE_LENGTH_MAX, FORCE_LENGTH_MAX);
                Gizmos.color = reactionForce.Relation == ChargedForce.RelationType.Attract ? ATTRACTION_FORCE_COLOR : REPULSION_FORCE_COLOR;
                Gizmos.DrawRay(transform.position, Vector2.ClampMagnitude(reactionForce.Vector, clampedMagnitude));
            }
        }

        void DrawNetReactionForce()
        {
            Vector2 netReactionForce = GetController().NetReactionForce;
            float clampedMagnitude = Method.Map(netReactionForce.magnitude, -MagneticForce.MAX_FORCE, MagneticForce.MAX_FORCE, -FORCE_LENGTH_MAX, FORCE_LENGTH_MAX);
            Gizmos.color = NET_FORCE_COLOR;
            Gizmos.DrawRay(transform.position, Vector2.ClampMagnitude(netReactionForce, clampedMagnitude));
        }

        void DrawEmissionRadius()
        {
            Gizmos.color = EMISSION_RADIUS_COLOR;
            Gizmos.DrawWireSphere(transform.position, GetValues().EmissionRadius);
        }
    }
}