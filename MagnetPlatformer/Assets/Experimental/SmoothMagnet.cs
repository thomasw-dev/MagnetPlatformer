using UnityEngine;

namespace Experimental
{
    public class SmoothMagnet : MonoBehaviour
    {
        [SerializeField] float _pushVelocity = 2.5f;
        [SerializeField] float _maxVelocity = 15.0f;
        [SerializeField] float _maxForce = 40.0f;
        [SerializeField] float _gain = 5f;

        [Space(10)]

        [SerializeField] bool _isActive = true;
        [SerializeField] bool _isAttracted = true;

        [Space(10)]

        [SerializeField] Transform[] _targets;

        public static float MAX_DISTANCE = 8f;

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
                    Vector2 distance = _targets[i].position - transform.position;
                    if (distance.magnitude > MAX_DISTANCE) { continue; }

                    Vector2 force = MagneticForce(distance);
                    Vector2 appliedForce = _isAttracted ? force : -force; // new Vector2(_maxForce, _maxForce) - force;
                    _rigidbody.AddForce(appliedForce);
                }
            }
        }

        Vector2 MagneticForce(Vector2 distance)
        {
            Vector2 targetVelocity = Vector2.ClampMagnitude(_pushVelocity * distance, _maxVelocity);
            Vector2 error = targetVelocity - _rigidbody.velocity;
            return Vector2.ClampMagnitude(_gain * error, _maxForce);
        }

        [ContextMenu("Reset")]
        void Reset()
        {
            _isActive = false;
            transform.position = _initialPos;
        }
    }
}