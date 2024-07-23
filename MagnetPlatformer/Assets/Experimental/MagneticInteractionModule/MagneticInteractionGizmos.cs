using UnityEditor;
using UnityEngine;

[RequireComponent(typeof(MagneticInteractionValues))]
public class MagneticInteractionGizmos : MonoBehaviour
{
    [Header("Gizmos Overrides")]
    public GizmosOverride.Type NetForce;
    public GizmosOverride.Type EmissionRadius;

    MagneticInteractionGizmosSettings _localSettings;
    public struct MagneticInteractionGizmosSettings
    {
        public GizmosOverride.Type NetForce;
        public GizmosOverride.Type EmissionRadius;
    }

    void OnValidate()
    {
        _localSettings.NetForce = NetForce;
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

    MagneticInteractionValues GetValues()
    {
        return GetComponent<MagneticInteractionValues>();
    }

    void OnDrawGizmos()
    {
        if (SettingEnabled(_localSettings.NetForce, DebugManager.MagneticInteractionGizmosSettings.NetForce))
            DrawNetForce();

        if (SettingEnabled(_localSettings.EmissionRadius, DebugManager.MagneticInteractionGizmosSettings.EmissionRadius))
            DrawEmissionRadius();

        void DrawNetForce()
        {
            const float LENGTH_MAX = 5f;
            float length = Method.Map(GetValues().Force, 0, MagneticForce.MAX_FORCE, 0, LENGTH_MAX);
            Gizmos.color = Color.cyan;
            Gizmos.DrawWireSphere(transform.position, length);
        }

        void DrawEmissionRadius()
        {
            Gizmos.color = Color.white;
            Gizmos.DrawWireSphere(transform.position, GetValues().Radius);
        }
    }
}