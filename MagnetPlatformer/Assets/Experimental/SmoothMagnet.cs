using UnityEngine;

namespace Experimental
{
    public static class Magnet
    {
        public enum Charge { Neutral, Positive, Negative }
    }

    public class SmoothMagnet : MonoBehaviour
    {
        // [SerializeField] float _pushVelocity = 2.5f;
        // [SerializeField] float _maxVelocity = 15.0f;
        // [SerializeField] float _maxForce = 40.0f;

        [SerializeField] bool _isActive = true;

        [SerializeField] Target[] _targets;

        Rigidbody2D _rigidbody;
        Gravity _gravity;
        Vector2 _initialPos;

        void Awake()
        {
            _rigidbody = GetComponent<Rigidbody2D>();
            _gravity = GetComponent<Gravity>();
        }

        void Start()
        {
            _initialPos = transform.position;
        }

        void FixedUpdate()
        {
            if (_isActive)
            {
                for (int i = 0; i < _targets.Length; i++)
                {
                    if (!_targets[i].gameObject.activeSelf) { continue; }

                    Vector2 distance = _targets[i].transform.position - transform.position;
                    if (distance.magnitude > _targets[i].Radius) { continue; }

                    if (_targets[i].Charge == Magnet.Charge.Neutral)
                    {
                        //_gravity.enabled = true;
                    }
                    else
                    {
                        //_gravity.enabled = false;
                        Vector2 force = MagneticForce.Calculate(_rigidbody.velocity, distance, _targets[i].Gain);
                        Vector2 appliedForce = MagneticForce.AdjustForceByCharge(force, _targets[i].Charge);
                        _rigidbody.AddForce(appliedForce);
                    }
                }
            }
        }

        [ContextMenu("Reset")]
        void Reset()
        {
            _isActive = false;
            transform.position = _initialPos;
        }
    }

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

        public static Vector2 Calculate(Vector2 selfRigidbodyVelocity, Vector2 selfTargetDistance, float targetGain)
        {
            Vector2 desiredVelocity = Vector2.ClampMagnitude(PUSH_VELOCITY * selfTargetDistance, MAX_VELOCITY);
            Vector2 deviation = desiredVelocity - selfRigidbodyVelocity;
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