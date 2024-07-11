using UnityEngine;

namespace Experimental
{
    public static class MagneticForce
    {
        const float PUSH_VELOCITY = 10f;
        const float MAX_VELOCITY = 100f;
        const float MAX_FORCE = 100f;

        public static Vector2 Calculate()
        {
            Vector2 output = Vector2.zero;
            return output;
        }

        public static Vector2 Calculate(Rigidbody2D selfRigidbody, Vector2 selfTargetDistance, float targetGain)
        {
            Vector2 desiredVelocity = Vector2.ClampMagnitude(PUSH_VELOCITY * selfTargetDistance, MAX_VELOCITY);
            Vector2 deviation = desiredVelocity - selfRigidbody.velocity;
            return Vector2.ClampMagnitude(targetGain * deviation, MAX_FORCE);
        }

        public static Vector2 AdjustForceByCharge(Vector2 force, Magnet.Charge targetCharge)
        {
            return targetCharge switch
            {
                Magnet.Charge.Neutral => Vector2.zero,
                Magnet.Charge.Positive => force,
                Magnet.Charge.Negative => -force,
                _ => Vector2.zero
            };
        }
    }
}