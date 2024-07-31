using System;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    // State

    public enum StateEnum { Idle, Chase, Return }
    public StateController<StateEnum> StateController = new StateController<StateEnum>();
    [SerializeField] StateEnum _state; // Inspector

    // Values

    [HideInInspector] public EnemyValues Values;

    // Chase Target

    [HideInInspector] public float MoveTargetX;
    const float DISTANCE_CLOSE_CUTOFF = 0.1f;
    GameObject _target;

    // Move Direction

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

    public Action OnKillPlayer;

    Rigidbody2D _rigidbody;

    // --------------------

    void Awake()
    {
        Values = GetComponent<EnemyValues>();
        _rigidbody = GetComponent<Rigidbody2D>();
        _target = Method.GetPlayerObject();
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
                //MoveTargetX = _player.transform.position.x;
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
        if (MoveDirection == Move.Direction.Left) move = Vector2.left * Values.Acceleration;
        if (MoveDirection == Move.Direction.Right) move = Vector2.right * Values.Acceleration;
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
            StateController.ChangeState(StateEnum.Idle);
        }
    }

    public void StartChase()
    {

    }

    public void StopChase()
    {

    }
}