using UnityEngine;

namespace Experimental
{
    public class SmoothMagnet : MonoBehaviour
    {
        [SerializeField] Transform target;
        [SerializeField] float _pushVelocity = 2.5f;
        [SerializeField] float _maxVelocity = 15.0f;
        [SerializeField] float _maxForce = 40.0f;
        [SerializeField] float _gain = 5f;
        [Space(10)]
        [SerializeField] bool _isActive = true;

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
                Vector2 distance = target.position - transform.position;
                Vector2 targetVelocity = Vector2.ClampMagnitude(_pushVelocity * distance, _maxVelocity);
                Vector2 error = targetVelocity - _rigidbody.velocity;
                Vector2 force = Vector2.ClampMagnitude(_gain * error, _maxForce);
                _rigidbody.AddForce(force);
            }
        }

        [ContextMenu("Reset")]
        void Reset()
        {
            _isActive = false;
            transform.position = _initialPos;
        }
    }
}