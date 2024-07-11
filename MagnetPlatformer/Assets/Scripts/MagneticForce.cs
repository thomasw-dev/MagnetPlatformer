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

        public static Vector2 Calculate(Rigidbody2D rigidbody, Vector2 distance, float gain)
        {
            Vector2 targetVelocity = Vector2.ClampMagnitude(PUSH_VELOCITY * distance, MAX_VELOCITY);
            Vector2 error = targetVelocity - rigidbody.velocity;
            return Vector2.ClampMagnitude(gain * error, MAX_FORCE);
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