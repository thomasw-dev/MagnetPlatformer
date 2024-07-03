using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField] GameObject _body;
    Rigidbody2D _rigidbody;

    [Header("Movement")]
    [Range(0.001f, 10f)]
    [SerializeField] float _moveSpeedProgressive = 1f;
    [Range(1f, 10f)]
    [SerializeField] float _moveSpeedConstant = 1f;

    [Header("Jump")]
    [Range(1f, 100f)]
    [SerializeField] float _jumpForce = 1f;

    const float MOVE_SPEED_MAX = 10f;

    void Start()
    {
        _rigidbody = _body.GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        float x = 0;

        /*if (UserInput.Jump())
        {
            velocity = new Vector2(_rigidbody.velocity.x, _jumpSpeed);
        }*/

        if (UserInput.IsMovingLeft())
        {
            // Progressive
            x = Mathf.Clamp(_rigidbody.velocity.x - _moveSpeedProgressive, -MOVE_SPEED_MAX, 0);
            // Constant
            //x = -_moveSpeedConstant;
        }

        if (UserInput.IsMovingRight())
        {
            // Progressive
            x = Mathf.Clamp(_rigidbody.velocity.x + _moveSpeedProgressive, 0, MOVE_SPEED_MAX);
            // Constant
            //x = _moveSpeedConstant;
        }

        _rigidbody.velocity = new Vector2(x, _rigidbody.velocity.y); //Vector2.ClampMagnitude(velocity, MOVE_SPEED_MAX);
    }
}