using UnityEngine;

public static class MagneticForce
{
    const float PUSH_VELOCITY = 10f;
    const float MAX_VELOCITY = 100f;
    const float MAX_FORCE = 100f;

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
}