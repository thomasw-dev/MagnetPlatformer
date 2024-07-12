using Experimental;
using UnityEngine;

namespace NewMagneticForce
{
    public class MagneticObject : MonoBehaviour
    {
        [SerializeField] SpriteRenderer _spriteRenderer;
        [SerializeField] bool _isActive = true;

        [Space(10)]

        [SerializeField] MagneticObject[] _targets;

        [Space(10)]

        [Range(0f, 5f)]
        public float Gain = 1f;

        [Range(0f, 10f)]
        public float Radius = 5f;

        [Space(10)]

        [SerializeField] bool _neutral = true;
        [SerializeField] bool _positive = false;
        [SerializeField] bool _negative = false;
        [HideInInspector] public Magnet.Charge Charge;

        Rigidbody2D _rigidbody;
        Vector2 _initialPos;

        Color _neutralColor = new Color(1.0f, 1.0f, 1.0f, 1.0f);
        Color _positiveColor = new Color(0.67f, 0, 0, 1.0f);
        Color _negativeColor = new Color(0, 0, 0.67f, 1.0f);

        void Awake()
        {
            _rigidbody = GetComponent<Rigidbody2D>();
        }

        void Start()
        {
            _initialPos = transform.position;
        }

        void OnValidate()
        {
            if (Charge == Magnet.Charge.Neutral)
            {
                if (_positive && !_negative) Charge = Magnet.Charge.Positive;
                if (!_positive && _negative) Charge = Magnet.Charge.Negative;
            }

            else if (Charge == Magnet.Charge.Positive)
            {
                if (_neutral && !_negative) Charge = Magnet.Charge.Neutral;
                if (!_neutral && _negative) Charge = Magnet.Charge.Negative;
            }

            else if (Charge == Magnet.Charge.Negative)
            {
                if (_neutral && !_positive) Charge = Magnet.Charge.Neutral;
                if (!_neutral && _positive) Charge = Magnet.Charge.Positive;
            }

            _neutral = Charge == Magnet.Charge.Neutral;
            _positive = Charge == Magnet.Charge.Positive;
            _negative = Charge == Magnet.Charge.Negative;

            UpdateChargeBools(Charge);
            UpdateColor(Charge);
        }

        void FixedUpdate()
        {
            GravityForce();

            if (!_isActive) { return; }

            for (int i = 0; i < _targets.Length; i++)
            {
                if (!_targets[i].gameObject.activeSelf) { continue; }

                Vector2 distance = _targets[i].transform.position - transform.position;
                if (distance.magnitude > _targets[i].Radius) { continue; }

                if (_targets[i].Charge != Magnet.Charge.Neutral)
                {
                    Vector2 force = Magnetic.Calculate(_rigidbody.velocity, distance, _targets[i].Gain);
                    Vector2 appliedForce = Magnetic.AdjustForceByCharge(force, Charge, _targets[i].Charge);
                    _rigidbody.AddForce(appliedForce);
                }
            }
        }

        void GravityForce()
        {
            Vector2 gravityForce = MagneticForce.Calculate(_rigidbody.velocity, Vector2.down, 1f);
            _rigidbody.AddForce(gravityForce);
        }

        [ContextMenu("Reset")]
        void Reset()
        {
            _isActive = false;
            transform.position = _initialPos;
        }

        void UpdateChargeBools(Magnet.Charge charge)
        {
            _neutral = charge == Magnet.Charge.Neutral;
            _positive = charge == Magnet.Charge.Positive;
            _negative = charge == Magnet.Charge.Negative;
        }

        void UpdateColor(Magnet.Charge charge)
        {
            if (charge == Magnet.Charge.Neutral) _spriteRenderer.color = _neutralColor;
            if (charge == Magnet.Charge.Positive) _spriteRenderer.color = _positiveColor;
            if (charge == Magnet.Charge.Negative) _spriteRenderer.color = _negativeColor;
        }

        void OnDrawGizmos()
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(transform.position, Radius);
        }
    }

    public static class Magnetic
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
            if (selfCharge == Magnet.Charge.Positive && targetCharge == Magnet.Charge.Positive) return -force;
            if (selfCharge == Magnet.Charge.Positive && targetCharge == Magnet.Charge.Negative) return force;
            if (selfCharge == Magnet.Charge.Negative && targetCharge == Magnet.Charge.Positive) return force;
            if (selfCharge == Magnet.Charge.Negative && targetCharge == Magnet.Charge.Negative) return -force;
            return Vector2.zero;
        }
    }
}