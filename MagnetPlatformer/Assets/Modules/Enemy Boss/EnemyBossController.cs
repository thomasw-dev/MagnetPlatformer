using System;
using UnityEngine;

public class EnemyBossController : MonoBehaviour
{
    // State

    public enum StateEnum { Idle, Chase, Dash, Death }
    public StateController<StateEnum> StateController = new StateController<StateEnum>();
    [SerializeField] StateEnum _state; // Inspector

    // Values

    [HideInInspector] public EnemyBossValues Values;

    [Header("Dependencies")] // Required to be assigned in the Inspector

    [SerializeField] Rigidbody2D _rigidbody;

    [Header("Move Target")]
    public Transform MoveTarget;
    Vector3 _initialPos;
    Vector3 _targetPos;
    const float DISTANCE_CLOSE_CUTOFF = 0.1f;

    // Move Direction

    Move.Direction _moveDirection;
    public Move.Direction MoveDirection
    {
        get { return _moveDirection; }
        set
        {
            if (value != _moveDirection) { OnMoveDirectionChange?.Invoke(value); }
            _moveDirection = value;
        }
    }
    public event Action<Move.Direction> OnMoveDirectionChange;

    // Dash

    EnemyBossDash _enemyBossDash;

    // Kill

    public Action OnKillPlayer;

    [SerializeField] Vector3 vel;

    // --------------------

    void Awake()
    {
        Values = GetComponent<EnemyBossValues>();
        _enemyBossDash = GetComponent<EnemyBossDash>();
    }

    void OnEnable()
    {
        if (_enemyBossDash != null) _enemyBossDash.OnDashStart += EnterDashState;
        if (_enemyBossDash != null) _enemyBossDash.OnDashStop += ExitDashState;
        OnKillPlayer += ExitChaseState;
    }

    void OnDisable()
    {
        if (_enemyBossDash != null) _enemyBossDash.OnDashStart -= EnterDashState;
        if (_enemyBossDash != null) _enemyBossDash.OnDashStop -= ExitDashState;
        OnKillPlayer -= ExitChaseState;
    }

    void Start()
    {
        StateController.ChangeState(StateEnum.Idle);
        _initialPos = transform.position;

        if (MoveTarget != null)
        {
            _targetPos = MoveTarget.position;
        }
    }

    void Update()
    {
        _state = StateController.CurrentEnum; // Inspector

        vel = _rigidbody.velocity;
    }

    void FixedUpdate()
    {
        if (StateController.CurrentEnum == StateEnum.Idle)
        {
            MoveDirection = Move.Direction.None;
        }

        if (StateController.CurrentEnum == StateEnum.Chase || StateController.CurrentEnum == StateEnum.Dash)
        {
            _targetPos = MoveTarget == null ? _initialPos : MoveTarget.transform.position;
            MoveDirection = UpdateMoveDirection(transform.position.x, _targetPos.x);
            MoveRigidbody(Values.ChaseAcceleration);
        }
    }

    public void CheckMoveDirection() => MoveDirection = UpdateMoveDirection(transform.position.x, _targetPos.x);

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

    // Chase

    public void EnterChase()
    {
        if (GameState.CurrentState != GameState.Play) { return; }
        if (StateController.CurrentEnum == StateEnum.Chase) { return; }

        EnterChaseState();
    }

    public void StayChase()
    {
        if (GameState.CurrentState != GameState.Play) { return; }
        if (StateController.CurrentEnum == StateEnum.Chase) { return; }

        EnterChaseState();
    }

    void EnterChaseState()
    {
        StateController.ChangeState(StateEnum.Chase);
        MoveDirection = UpdateMoveDirection(transform.position.x, _targetPos.x);
    }

    public void ExitChaseState() => StateController.ChangeState(StateEnum.Idle);

    // Dash

    void EnterDashState() => StateController.ChangeState(StateEnum.Dash);

    void ExitDashState() => StateController.ChangeState(StateEnum.Chase);
}
