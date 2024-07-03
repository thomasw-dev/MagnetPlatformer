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
        UserInput.OnMoveLeftInput += MoveLeft;
        UserInput.OnMoveRightInput += MoveRight;
        UserInput.OnJumpInput += Jump;
    }

    void OnDisable()
    {

        UserInput.OnMoveLeftInput += MoveLeft;
        UserInput.OnMoveRightInput += MoveRight;
        UserInput.OnJumpInput += Jump;
    }

    void Start()
    {
        _rigidbody = _body.GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        // ...
    }

    void MoveLeft()
    {
        float x = Mathf.Clamp(_rigidbody.velocity.x - _moveSpeed, -MOVE_SPEED_MAX, 0);
        _rigidbody.velocity = new Vector2(x, _rigidbody.velocity.y);
    }

    void MoveRight()
    {
        float x = Mathf.Clamp(_rigidbody.velocity.x + _moveSpeed, 0, MOVE_SPEED_MAX);
        _rigidbody.velocity = new Vector2(x, _rigidbody.velocity.y);
    }
    void Jump()
    {
        _rigidbody.AddForce(Vector2.up * _jumpForce);
    }
}