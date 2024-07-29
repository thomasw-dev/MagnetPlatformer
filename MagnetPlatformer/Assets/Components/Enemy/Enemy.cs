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
    [SerializeField] EnemyActiveArea _activeArea;
    [SerializeField] GameObject _player;

    [HideInInspector] public float MoveTargetX;

    [Range(1f, 10f)]
    [SerializeField] float _moveSpeed = 1.0f;

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
    }

    void OnEnable()
    {
        _activeArea.OnPlayerEnter += Chase;
        _activeArea.OnPlayerExit += Idle;
        GameState.Play.OnExit += ExitPlay;
    }

    void OnDisable()
    {
        _activeArea.OnPlayerEnter -= Chase;
        _activeArea.OnPlayerExit -= Idle;
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
            if (StateController.CurrentEnum == StateEnum.Idle)
            {
                IdleMovement();
            }

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
        OnMoveDirectionChange.Invoke(Move.Direction.None);
    }

    void Chase()
    {
        StateController.ChangeState(StateEnum.Chase);
        OnMoveDirectionChange.Invoke(MoveDirection);
    }

    void IdleMovement()
    {
        //_rigidbody.velocity = Vector2.zero;
    }

    void ChaseMovement()
    {
        Vector2 move = Vector2.zero;
        if (MoveDirection == Move.Direction.None) move = Vector2.zero;
        if (MoveDirection == Move.Direction.Left) move = Vector2.left * _moveSpeed;
        if (MoveDirection == Move.Direction.Right) move = Vector2.right * _moveSpeed;
        _rigidbody.velocity = move;
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

    void OnTriggerEnter2D(Collider2D col)
    {
        if (Method.IsPlayerObject(col.gameObject))
        {
            GameEvent.Raise(GameEvent.Event.Death);
        }
    }

    void ExitPlay() => StateController.ChangeState(StateEnum.Idle);

    void OnDrawGizmosSelected()
    {
        _activeArea.DrawGizmos();
    }
}