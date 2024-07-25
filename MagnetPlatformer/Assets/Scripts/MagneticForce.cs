using System;
using UnityEngine;

public static class MagneticForce
{
    public const float PUSH_VELOCITY = 10f;
    public const float MAX_VELOCITY = 100f;
    public const float MAX_FORCE = 100f;

    public static Vector2 Calculate(Vector2 selfRigidbodyVelocity, Vector2 selfTargetDistance, float targetGain)
    {
        Vector2 desiredVelocity = Vector2.ClampMagnitude(PUSH_VELOCITY * selfTargetDistance, MAX_VELOCITY);
        Vector2 deviation = desiredVelocity - selfRigidbodyVelocity;
        return Vector2.ClampMagnitude(targetGain * deviation, MAX_FORCE);
    }

    public static Vector2 AdjustForceByCharge(Vector2 force, Magnet.Charge selfCharge, Magnet.Charge targetCharge)
    {
        // Repel or Attract (1 = attract, -1 = repel)
        if (selfCharge == Magnet.Charge.Positive && targetCharge == Magnet.Charge.Positive) return force;
        if (selfCharge == Magnet.Charge.Positive && targetCharge == Magnet.Charge.Negative) return -force;
        if (selfCharge == Magnet.Charge.Negative && targetCharge == Magnet.Charge.Positive) return -force;
        if (selfCharge == Magnet.Charge.Negative && targetCharge == Magnet.Charge.Negative) return force;
        return Vector2.zero;
    }

    public static ChargedForce ConvertToChargedForce(Vector2 force, Magnet.Charge selfCharge, Magnet.Charge targetCharge)
    {
        if (selfCharge == Magnet.Charge.Positive && targetCharge == Magnet.Charge.Positive) return new ChargedForce { Relation = ChargedForce.RelationType.Repel, Vector = force };
        if (selfCharge == Magnet.Charge.Positive && targetCharge == Magnet.Charge.Negative) return new ChargedForce { Relation = ChargedForce.RelationType.Attract, Vector = -force };
        if (selfCharge == Magnet.Charge.Negative && targetCharge == Magnet.Charge.Positive) return new ChargedForce { Relation = ChargedForce.RelationType.Attract, Vector = -force };
        if (selfCharge == Magnet.Charge.Negative && targetCharge == Magnet.Charge.Negative) return new ChargedForce { Relation = ChargedForce.RelationType.Repel, Vector = force };
        return new ChargedForce { Relation = ChargedForce.RelationType.Neutral, Vector = Vector2.zero };
    }
}

[Serializable]
public struct ChargedForce
{
    public enum RelationType { Neutral, Attract, Repel }
    public RelationType Relation;
    public Vector2 Vector;
}