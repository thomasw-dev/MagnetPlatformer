using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    [Tooltip("These fields need their values to be assigned in the Inspector.")]
    [Header("Assign Fields")]
    [SerializeField] Transform _groundCheckRaycastPoint;
    [SerializeField] LayerMask _environmentLayerMask;

    [Header("Movement")]
    [Range(0.001f, 10f)]
    [SerializeField] float _moveSpeed = 1f;

    [Header("Jump")]
    [Range(1f, 1000f)]
    [SerializeField] float _jumpForce = 1f;

    [SerializeField] bool _airJumpEnabled = false;

    const float MOVE_SPEED_MAX = 10f;
    const float GROUND_CHECK_RAYCAST_LENGTH = 0.5f;

    Rigidbody2D _rigidbody2D;

    bool _isInputEnabled = true;
    bool _isMovingLeft = false;
    bool _isMovingRight = false;
    bool _isGrounded = false;
    float _velocityX;

    [Space(10)]
    [SerializeField] bool gizmos = false;

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
        _rigidbody2D = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        // _isInputEnabled is only true when GameState is Playing
        _isInputEnabled = GameManager.GameState == GameManager.State.Playing ? true : false;
    }

    void FixedUpdate()
    {
        if (_isMovingLeft || _isMovingRight)
        {
            _rigidbody2D.velocity = new Vector2(_velocityX, _rigidbody2D.velocity.y);
        }

        _isGrounded = GroundCheck();
    }

    bool GroundCheck()
    {
        Vector2 position = _groundCheckRaycastPoint.position;
        Vector2 direction = Vector2.down;
        RaycastHit2D hit = Physics2D.Raycast(position, direction, GROUND_CHECK_RAYCAST_LENGTH, _environmentLayerMask);
        return hit.collider != null ? true : false;
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

        bool canJump;
        if (_isGrounded) canJump = true;
        else canJump = _airJumpEnabled ? true : false;

        if (canJump)
        {
            _rigidbody2D.AddForce(Vector2.up * _jumpForce);
        }
    }

    void OnDrawGizmos()
    {
        if (gizmos)
        {
            Gizmos.color = Color.red;
            Vector3 start = _groundCheckRaycastPoint.position;
            Vector3 end = start + Vector3.down * GROUND_CHECK_RAYCAST_LENGTH;
            Gizmos.DrawRay(start, end - start);
        }
    }
}