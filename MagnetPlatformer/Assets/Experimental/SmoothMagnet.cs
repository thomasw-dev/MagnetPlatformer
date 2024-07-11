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

                    Vector2 force = MagneticForce.Calculate(_rigidbody, distance, _targets[i].Gain);
                    Vector2 appliedForce = MagneticForce.AdjustForceByCharge(force, _targets[i].Charge);
                    _rigidbody.AddForce(appliedForce);
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
}