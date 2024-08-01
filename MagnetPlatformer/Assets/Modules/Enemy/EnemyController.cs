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

    [Header("Dependencies")] // Required to be assigned in the Inspector

    [SerializeField] Rigidbody2D _rigidbody;

    // Chase Target

    Transform _target;
    Vector3 _targetPos;
    Vector3 _initialPos;
    const float DISTANCE_CLOSE_CUTOFF = 0.1f;

    public Vector3 TargetPoint { get => _targetPos; }

    // Move Direction

    Move.Direction _moveDirection;
    public Move.Direction MoveDirection
    {
        get { return _moveDirection; }
        set
        {
            if (value != _moveDirection) OnMoveDirectionChange?.Invoke(value);
            _moveDirection = value;
        }
    }
    public event Action<Move.Direction> OnMoveDirectionChange;

    public Action OnKillPlayer;

    // --------------------

    void Awake()
    {
        Values = GetComponent<EnemyValues>();
    }

    void OnEnable()
    {
        OnKillPlayer += ExitChase;
    }

    void OnDisable()
    {
        OnKillPlayer -= ExitChase;
    }

    void Start()
    {
        StateController.ChangeState(StateEnum.Idle);
        _target = Method.GetPlayerObject().transform;
    }

    void Update()
    {
        _state = StateController.CurrentEnum; // Inspector

        if (StateController.CurrentEnum == StateEnum.Idle)
        {
            if (Values.ReturnToInitialPosition)
            {
                _targetPos = _initialPos;
                if (Mathf.Abs(transform.position.x - _targetPos.x) >= DISTANCE_CLOSE_CUTOFF)
                {
                    StateController.ChangeState(StateEnum.Return);
                }
            }
            else MoveDirection = Move.Direction.None;
        }

        if (StateController.CurrentEnum == StateEnum.Chase)
        {
            _targetPos = _target.transform.position;
            MoveDirection = UpdateMoveDirection(transform.position.x, _targetPos.x);
            MoveRigidbody(Values.ChaseAcceleration);
        }

        if (StateController.CurrentEnum == StateEnum.Return)
        {
            MoveDirection = UpdateMoveDirection(transform.position.x, _targetPos.x);
            MoveRigidbody(Values.WalkAcceleration);
        }
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

    void MoveRigidbody(float acceleration)
    {
        Vector2 moveForce = Vector2.zero;
        if (MoveDirection == Move.Direction.None) moveForce = Vector2.zero;
        if (MoveDirection == Move.Direction.Left) moveForce = Vector2.left * acceleration;
        if (MoveDirection == Move.Direction.Right) moveForce = Vector2.right * acceleration;
        _rigidbody.AddForce(moveForce);
    }

    public void EnterChase()
    {
        if (GameState.CurrentState != GameState.Play) { return; }

        _target = Method.GetPlayerObject().transform;
        StateController.ChangeState(StateEnum.Chase);
    }

    public void StayChase()
    {
        if (GameState.CurrentState != GameState.Play) { return; }

        if (StateController.CurrentEnum != StateEnum.Chase)
        {
            _target = Method.GetPlayerObject().transform;
            StateController.ChangeState(StateEnum.Chase);
        }
    }

    public void ExitChase()
    {
        _targetPos = _initialPos;

        if (Values.ReturnToInitialPosition)
            StateController.ChangeState(StateEnum.Return);
        else StateController.ChangeState(StateEnum.Idle);
    }
}