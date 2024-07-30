using System;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public enum StateEnum
    {
        Idle, Chase
    }
    public StateController<StateEnum> StateController = new StateController<StateEnum>();
    [SerializeField] StateEnum _state; // Inspector
    [SerializeField] EnemyRadiusArea _radiusArea;

    GameObject _player;

    [HideInInspector] public float MoveTargetX;

    [Range(1f, 20f)]
    public float ChaseRadius = 10f;

    [Range(1f, 10f)]
    [SerializeField] float _accelerationForce = 1.0f;

    [Range(1f, 10f)]
    [SerializeField] float _maxSpeed = 1.0f;

    Move.Direction _moveDirection;
    public Move.Direction MoveDirection
    {
        get { return _moveDirection; }
        set
        {
            if (value != _moveDirection) OnMoveDirectionChange.Invoke(value);
            _moveDirection = value;
        }
    }
    public event Action<Move.Direction> OnMoveDirectionChange;

    const float DISTANCE_CLOSE_CUTOFF = 0.1f;

    Rigidbody2D _rigidbody;

    void Awake()
    {
        _rigidbody = GetComponent<Rigidbody2D>();
        _player = Method.GetPlayerObject();
    }

    void OnValidate()
    {
        _radiusArea.GetComponent<CircleCollider2D>().radius = ChaseRadius;
    }

    void OnEnable()
    {
        _radiusArea.OnPlayerEnter += Chase;
        _radiusArea.OnPlayerExit += Idle;
        GameState.Play.OnExit += ExitPlay;
    }

    void OnDisable()
    {
        _radiusArea.OnPlayerEnter -= Chase;
        _radiusArea.OnPlayerExit -= Idle;
        GameState.Play.OnExit -= ExitPlay;
    }

    void Start()
    {
        StateController.ChangeState(StateEnum.Idle);
    }

    void Update()
    {
        _state = StateController.CurrentEnum; // Inspector

        if (GameState.CurrentState == GameState.Play)
        {
            if (StateController.CurrentEnum == StateEnum.Chase)
            {
                MoveTargetX = _player.transform.position.x;
                MoveDirection = UpdateMoveDirection(transform.position.x, MoveTargetX);
                ChaseMovement();
            }
        }
    }

    void Idle()
    {
        StateController.ChangeState(StateEnum.Idle);
        OnMoveDirectionChange?.Invoke(Move.Direction.None);
    }

    void Chase()
    {
        StateController.ChangeState(StateEnum.Chase);
        OnMoveDirectionChange?.Invoke(MoveDirection);
    }

    void ChaseMovement()
    {
        Vector2 move = Vector2.zero;
        if (MoveDirection == Move.Direction.None) move = Vector2.zero;
        if (MoveDirection == Move.Direction.Left) move = Vector2.left * _accelerationForce;
        if (MoveDirection == Move.Direction.Right) move = Vector2.right * _accelerationForce;
        _rigidbody.AddForce(move);

        //_rigidbody.velocity = Vector2.ClampMagnitude(_rigidbody.velocity, _maxSpeed);
    }

    Move.Direction UpdateMoveDirection(float selfX, float targetX)
    {
        if (Mathf.Abs(selfX - targetX) >= DISTANCE_CLOSE_CUTOFF)
        {
            if (selfX >= targetX)
                return Move.Direction.Left;
            else return Move.Direction.Right;
        }
        else return Move.Direction.None;
    }

    void ExitPlay() => StateController.ChangeState(StateEnum.Idle);

    void OnDrawGizmosSelected()
    {
        _radiusArea.DrawGizmos(ChaseRadius);
    }
}