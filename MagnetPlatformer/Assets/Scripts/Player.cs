using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField] GameObject _body;
    Rigidbody2D _rigidbody;

    [Header("Movement")]
    [Range(0.001f, 10f)]
    [SerializeField] float _moveSpeed = 1f;

    [Header("Jump")]
    [Range(1f, 1000f)]
    [SerializeField] float _jumpForce = 1f;

    const float MOVE_SPEED_MAX = 10f;

    void OnEnable()
    {

    }

    void OnDisable()
    {

    }

    void Start()
    {
        _rigidbody = _body.GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        /*if (UserInput.Jump())
        {
            _rigidbody.AddForce(Vector2.up * _jumpForce);
            //velocity = new Vector2(_rigidbody.velocity.x, _jumpSpeed);
        }*/

        float x = 0;

        if (UserInput.IsMovingLeft())
        {
            x = Mathf.Clamp(_rigidbody.velocity.x - _moveSpeed, -MOVE_SPEED_MAX, 0);
        }

        if (UserInput.IsMovingRight())
        {
            x = Mathf.Clamp(_rigidbody.velocity.x + _moveSpeed, 0, MOVE_SPEED_MAX);
        }

        _rigidbody.velocity = new Vector2(x, _rigidbody.velocity.y);
    }

    void DoJump()
    {

    }
}