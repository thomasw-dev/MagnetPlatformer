using UnityEngine;

public class Player : MonoBehaviour
{
    [Tooltip("These fields need their values to be assigned in the Inspector.")]
    [Header("Assign Fields")]
    [SerializeField] GroundCheck _groundCheck;

    [Header("Movement")]
    [Range(0.001f, 10f)]
    [SerializeField] float _moveSpeed = 1f;

    [Header("Jump")]
    [Range(1f, 10000f)]
    [SerializeField] float _jumpForce = 1f;

    [SerializeField] bool _airJumpEnabled = false;

    const float MOVE_SPEED_MAX = 15f;

    Rigidbody2D _rigidbody2D;

    [SerializeField] bool _isGrounded = false;
    bool _isMovingLeft = false;
    bool _isMovingRight = false;
    [SerializeField] bool _isJumping = false;
    float _velocityX;

    void OnEnable()
    {
        GameState.Initialize.OnEnter += Initialize;
        GameState.Play.OnEnter += EnterPlay;
        GameState.Play.OnExit += ResetInput;

        InputManager.OnMoveLeftInput += MoveLeft;
        InputManager.OnMoveLeftInputStop += MoveLeftStop;
        InputManager.OnMoveRightInput += MoveRight;
        InputManager.OnMoveRightInputStop += MoveRightStop;

        InputManager.OnJumpInput += Jump;
        InputManager.OnJumpInputStop += JumpStop;
        InputManager.OnJumpAltInput += Jump;
        InputManager.OnJumpAltInputStop += JumpStop;
    }

    void OnDisable()
    {
        GameState.Initialize.OnEnter -= Initialize;
        GameState.Play.OnEnter -= EnterPlay;
        GameState.Play.OnExit -= ResetInput;

        InputManager.OnMoveLeftInput -= MoveLeft;
        InputManager.OnMoveLeftInputStop -= MoveLeftStop;
        InputManager.OnMoveRightInput -= MoveRight;
        InputManager.OnMoveRightInputStop -= MoveRightStop;

        InputManager.OnJumpInput -= Jump;
        InputManager.OnJumpInputStop -= JumpStop;
        InputManager.OnJumpAltInput -= Jump;
        InputManager.OnJumpAltInputStop -= JumpStop;
    }

    void Awake()
    {
        _rigidbody2D = GetComponent<Rigidbody2D>();
    }

    void FixedUpdate()
    {
        if (_isMovingLeft || _isMovingRight)
        {
            _rigidbody2D.velocity = new Vector2(_velocityX, _rigidbody2D.velocity.y);
        }

        _isGrounded = _groundCheck == null ? false : _groundCheck.IsGrounded;
    }

    void Initialize()
    {
        _rigidbody2D.bodyType = RigidbodyType2D.Kinematic;
    }

    void EnterPlay()
    {
        _rigidbody2D.bodyType = RigidbodyType2D.Dynamic;
    }

    void ResetInput()
    {
        _isGrounded = false;
        _isMovingLeft = false;
        _isMovingRight = false;
        _isJumping = false;
        _rigidbody2D.velocity = Vector2.zero;
    }

    void MoveLeft()
    {
        _isMovingLeft = true;
        _velocityX = Mathf.Clamp(_rigidbody2D.velocity.x - _moveSpeed, -MOVE_SPEED_MAX, 0);
    }

    void MoveRight()
    {
        _isMovingRight = true;
        _velocityX = Mathf.Clamp(_rigidbody2D.velocity.x + _moveSpeed, 0, MOVE_SPEED_MAX);
    }

    void MoveLeftStop()
    {
        _isMovingLeft = false;
    }
    void MoveRightStop()
    {
        _isMovingRight = false;
    }

    void Jump()
    {
        if (_airJumpEnabled || (_isGrounded && !_isJumping))
        {
            _rigidbody2D.AddForce(Vector2.up * _jumpForce);
            _isJumping = true;
        }
    }

    void JumpStop()
    {
        _isJumping = false;
    }
}