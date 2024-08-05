using UnityEngine;

public class Player : MonoBehaviour
{
    [Tooltip("These fields need their values to be assigned in the Inspector.")]
    [Header("Assign Fields")]
    [SerializeField] Transform _groundCheckRaycastPoint;

    [Header("Movement")]
    [Range(0.001f, 10f)]
    [SerializeField] float _moveSpeed = 1f;

    [Header("Jump")]
    [Range(1f, 10000f)]
    [SerializeField] float _jumpForce = 1f;

    [SerializeField] bool _airJumpEnabled = false;

    const float MOVE_SPEED_MAX = 15f;
    const float GROUND_CHECK_RAYCAST_LENGTH = 0.5f;
    LayerMask[] _groundLayers;

    Rigidbody2D _rigidbody2D;

    bool _isInputEnabled = true;
    bool _isMovingLeft = false;
    bool _isMovingRight = false;
    bool _isGrounded = false;
    bool _isJumping = false;
    float _velocityX;

    [Space(10)]
    [SerializeField] bool gizmos = false;

    void OnEnable()
    {
        GameState.Initialize.OnEnter += Initialize;
        GameState.Play.OnEnter += EnableInput;
        GameState.Play.OnExit += DisableInput;
        GameState.Win.OnEnter += DisableInput;

        InputManager.OnMoveLeftInput += MoveLeft;
        InputManager.OnMoveLeftInputStop += MoveLeftStop;
        InputManager.OnMoveRightInput += MoveRight;
        InputManager.OnMoveRightInputStop += MoveRightStop;
        InputManager.OnJumpInput += Jump;
    }

    void OnDisable()
    {
        GameState.Initialize.OnEnter -= Initialize;
        GameState.Play.OnEnter -= EnableInput;
        GameState.Play.OnExit -= DisableInput;
        GameState.Win.OnEnter -= DisableInput;

        InputManager.OnMoveLeftInput -= MoveLeft;
        InputManager.OnMoveLeftInputStop -= MoveLeftStop;
        InputManager.OnMoveRightInput -= MoveRight;
        InputManager.OnMoveRightInputStop -= MoveRightStop;
        InputManager.OnJumpInput -= Jump;
    }

    void Awake()
    {
        _rigidbody2D = GetComponent<Rigidbody2D>();

        _groundLayers = new LayerMask[]
        {
            LayerMask.GetMask(Constants.LAYER.Physics.ToString()),
            LayerMask.GetMask(Constants.LAYER.Environment.ToString())
        };
    }

    void Update()
    {
        // _isInputEnabled is only true when GameState is Playing
        _isInputEnabled = GameState.CurrentState == GameState.Play ? true : false;
    }

    void FixedUpdate()
    {
        if (_isMovingLeft || _isMovingRight)
        {
            _rigidbody2D.velocity = new Vector2(_velocityX, _rigidbody2D.velocity.y);
        }

        // Just touched the ground, player can jump again
        if (!_isGrounded && GroundCheck())
        {
            _isJumping = false;
        }
        _isGrounded = GroundCheck();
    }

    bool GroundCheck()
    {
        foreach (LayerMask layerMask in _groundLayers)
        {
            RaycastHit2D hit = Physics2D.Raycast(_groundCheckRaycastPoint.position, Vector2.down, GROUND_CHECK_RAYCAST_LENGTH, layerMask);
            if (hit.collider != null)
            {
                return true;
            }
        }
        return false;
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
        if (_isGrounded && !_isJumping) canJump = true;
        else canJump = _airJumpEnabled ? true : false;

        if (canJump)
        {
            _rigidbody2D.AddForce(Vector2.up * _jumpForce);
            _isJumping = true;
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