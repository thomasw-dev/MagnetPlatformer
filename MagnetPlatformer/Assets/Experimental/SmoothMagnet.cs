using UnityEngine;

namespace Experimental
{
    public static class Magnet
    {
        public enum Charge { Neutral, Positive, Negative }
    }

    public class SmoothMagnet : MonoBehaviour
    {
        [SerializeField] float _pushVelocity = 2.5f;
        [SerializeField] float _maxVelocity = 15.0f;
        [SerializeField] float _maxForce = 40.0f;

        [Space(10)]

        [SerializeField] bool _isActive = true;

        [Space(10)]

        [SerializeField] Target[] _targets;

        Rigidbody2D _rigidbody;
        Vector2 _initialPos;

        void Awake()
        {
            _rigidbody = GetComponent<Rigidbody2D>();
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

                    Vector2 force = MagneticForce(distance, _targets[i].Gain);
                    Vector2 appliedForce;
                    switch (_targets[i].Charge)
                    {
                        case Magnet.Charge.Neutral: appliedForce = Vector2.zero;
                            break;
                        case Magnet.Charge.Positive: appliedForce = force;
                            break;
                        case Magnet.Charge.Negative: appliedForce = -force;
                            break;
                        default: appliedForce = Vector2.zero;
                            break;
                    }

                    _rigidbody.AddForce(appliedForce);
                }
            }
        }

        Vector2 MagneticForce(Vector2 distance, float gain)
        {
            Vector2 targetVelocity = Vector2.ClampMagnitude(_pushVelocity * distance, _maxVelocity);
            Vector2 error = targetVelocity - _rigidbody.velocity;
            return Vector2.ClampMagnitude(gain * error, _maxForce);
        }

        [ContextMenu("Reset")]
        void Reset()
        {
            _isActive = false;
            transform.position = _initialPos;
        }
    }
}