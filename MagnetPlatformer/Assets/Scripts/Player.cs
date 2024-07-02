using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField] GameObject _body;
    Rigidbody2D _rigidbody;

    [Range(1f, 100f)]
    [SerializeField] float _moveSpeed = 1f;

    [Range(1f, 100f)]
    [SerializeField] float _jumpSpeed = 1f;

    const float MOVE_SPEED_MAX = 10f;

    void Start()
    {
        _rigidbody = _body.GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        Vector2 velocity = Vector2.zero;

        if (Input.GetKeyDown(KeyCode.Space))
        {
            velocity = new Vector2(_rigidbody.velocity.x, _jumpSpeed);
        }

        if (Input.GetKey(KeyCode.A))
        {
            velocity = new Vector2(_rigidbody.velocity.x - _moveSpeed, _rigidbody.velocity.y);
        }

        if (Input.GetKey(KeyCode.D))
        {
            velocity = new Vector2(_rigidbody.velocity.x + _moveSpeed, _rigidbody.velocity.y);
        }

        _rigidbody.velocity = Vector2.ClampMagnitude(velocity, MOVE_SPEED_MAX);
    }
}