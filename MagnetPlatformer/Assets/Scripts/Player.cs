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

    bool isMovingLeft = false;
    bool isMovingRight = false;

    float _velocityX;

    void OnEnable()
    {
        UserInput.OnMoveLeftInput += MoveLeft;
        UserInput.OnMoveLeftInputStop += MoveLeftStop;
        UserInput.OnMoveRightInput += MoveRight;
        UserInput.OnMoveRightInputStop += MoveRightStop;
        UserInput.OnJumpInput += Jump;
    }

    void OnDisable()
    {
        UserInput.OnMoveLeftInput -= MoveLeft;
        UserInput.OnMoveLeftInputStop -= MoveLeftStop;
        UserInput.OnMoveRightInput -= MoveRight;
        UserInput.OnMoveRightInputStop -= MoveRightStop;
        UserInput.OnJumpInput -= Jump;
    }

    void Start()
    {
        _rigidbody = _body.GetComponent<Rigidbody2D>();
    }

    void FixedUpdate()
    {
        if (isMovingLeft || isMovingRight)
        {
            _rigidbody.velocity = new Vector2(_velocityX, _rigidbody.velocity.y);
        }
    }

    void MoveLeft()
    {
        isMovingLeft = true;
        _velocityX = Mathf.Clamp(_rigidbody.velocity.x - _moveSpeed, -MOVE_SPEED_MAX, 0);
    }

    void MoveRight()
    {
        isMovingRight = true;
        _velocityX = Mathf.Clamp(_rigidbody.velocity.x + _moveSpeed, 0, MOVE_SPEED_MAX);
    }

    void MoveLeftStop() => isMovingLeft = false;
    void MoveRightStop() => isMovingRight = false;

    void Jump()
    {
        _rigidbody.AddForce(Vector2.up * _jumpForce);
    }
}