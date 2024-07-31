using System;
using UnityEngine;

public static class MagneticInteractionPhysics
{
    public const float DISTANCE_MULTIPLIER = 5f;

    public static Vector2 Calculate(Vector2 selfPos, Vector2 targetPos, float targetGain)
    {
        Vector2 direction = selfPos - targetPos;
        float distance = direction.magnitude;
        float forceMagnitude = 1 / distance * Mathf.Pow(DISTANCE_MULTIPLIER + targetGain, 6);
        return direction.normalized * forceMagnitude;
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