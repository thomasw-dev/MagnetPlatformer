using UnityEngine;

namespace Experimental
{
    public class Gravity : MonoBehaviour
    {
        Rigidbody2D _rigidbody;

        void Awake()
        {
            _rigidbody = GetComponent<Rigidbody2D>();
        }

        void FixedUpdate()
        {
            Vector2 gravityForce = MagneticForce.Calculate(_rigidbody.velocity, Vector2.down, 1f);
            _rigidbody.AddForce(gravityForce);
        }
    }
}