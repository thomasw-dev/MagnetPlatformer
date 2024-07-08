using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField] GameObject _rigidbodyObject;
    Rigidbody2D _rigidbody2D;

    [Header("Movement")]
    [Range(0.001f, 10f)]
    [SerializeField] float _moveSpeed = 1f;

    [Header("Jump")]
    [Range(1f, 1000f)]
    [SerializeField] float _jumpForce = 1f;

    [SerializeField] bool _airJumpEnabled = false;

    const float MOVE_SPEED_MAX = 10f;

    bool _isInputEnabled = true;
    bool _isMovingLeft = false;
    bool _isMovingRight = false;
    bool isGrounded = false;

    float _velocityX;

    void OnEnable()
    {
        GameManager.OnInitializationEnter += Initialize;
        GameManager.OnPlayingEnter += EnableInput;
        GameManager.OnPlayingExit += DisableInput;
        GameManager.OnWinEnter += DisableInput;

        InputManager.OnMoveLeftInput += MoveLeft;
        InputManager.OnMoveLeftInputStop += MoveLeftStop;
        InputManager.OnMoveRightInput += MoveRight;
        InputManager.OnMoveRightInputStop += MoveRightStop;
        InputManager.OnJumpInput += Jump;
    }

    void OnDisable()
    {
        GameManager.OnInitializationEnter -= Initialize;
        GameManager.OnPlayingEnter -= EnableInput;
        GameManager.OnPlayingExit -= DisableInput;
        GameManager.OnWinEnter -= DisableInput;

        InputManager.OnMoveLeftInput -= MoveLeft;
        InputManager.OnMoveLeftInputStop -= MoveLeftStop;
        InputManager.OnMoveRightInput -= MoveRight;
        InputManager.OnMoveRightInputStop -= MoveRightStop;
        InputManager.OnJumpInput -= Jump;
    }

    void Awake()
    {
        _rigidbody2D = _rigidbodyObject.GetComponent<Rigidbody2D>();
    }

    void Start()
    {
        _rigidbody2D.freezeRotation = true;
    }

    void Update()
    {
        // _isInputEnabled is only true when GameState is Playing
        _isInputEnabled = GameManager.GameState == GameManager.State.Playing ? true : false;

        Debug.Log(isGrounded);
    }

    void FixedUpdate()
    {
        if (_isMovingLeft || _isMovingRight)
        {
            _rigidbody2D.velocity = new Vector2(_velocityX, _rigidbody2D.velocity.y);
        }
    }

    void OnCollisionEnter2D(Collision2D col)
    {
        //if (col.gameObject.tag == "Environment" || col.gameObject.tag == "Object")
        //{
            isGrounded = true;
        //}
    }

    void OnCollisionExit2D(Collision2D col)
    {
        //if (col.gameObject.tag == "Environment" || col.gameObject.tag == "Object")
        //{
            isGrounded = false;
        //}
    }

    void Initialize()
    {
        DisableInput();
        _rigidbody2D.bodyType = RigidbodyType2D.Kinematic;
    }

    void EnableInput()
    {
        _isInputEnabled = true;
        _rigidbody2D.bodyType = RigidbodyType2D.Dynamic;
    }

    void DisableInput()
    {
        _isInputEnabled = false;
        _isMovingLeft = false;
        _isMovingRight = false;
        _rigidbody2D.velocity = Vector2.zero;
    }

    void MoveLeft()
    {
        if (!_isInputEnabled) { return; }
        _isMovingLeft = true;
        _velocityX = Mathf.Clamp(_rigidbody2D.velocity.x - _moveSpeed, -MOVE_SPEED_MAX, 0);
    }

    void MoveRight()
    {
        if (!_isInputEnabled) { return; }
        _isMovingRight = true;
        _velocityX = Mathf.Clamp(_rigidbody2D.velocity.x + _moveSpeed, 0, MOVE_SPEED_MAX);
    }

    void MoveLeftStop()
    {
        if (!_isInputEnabled) { return; }
        _isMovingLeft = false;
    }
    void MoveRightStop()
    {
        if (!_isInputEnabled) { return; }
        _isMovingRight = false;
    }

    void Jump()
    {
        if (!_isInputEnabled) { return; }
        if (true) // (isGrounded || _airJumpEnabled)
        {
            _rigidbody2D.AddForce(Vector2.up * _jumpForce);
        }
    }
}