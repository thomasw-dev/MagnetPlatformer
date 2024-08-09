using System;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    // State

    public enum StateEnum { Idle, Chase, Return, Death }
    public StateController<StateEnum> StateController = new StateController<StateEnum>();
    [SerializeField] StateEnum _state; // Inspector

    // Values

    [HideInInspector] public EnemyValues Values;

    [Header("Dependencies")] // Required to be assigned in the Inspector

    [SerializeField] Rigidbody2D _rigidbody;

    [Header("Enemy Boss")]
    [SerializeField] Transform _enemyBossMoveTarget;

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
            if (value != _moveDirection) { OnMoveDirectionChange?.Invoke(value); }
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
        _initialPos = transform.position;

        // Enemy Boss
        if (IsEnemyBoss())
        {
            if (_enemyBossMoveTarget != null)
            {
                _target = _enemyBossMoveTarget;
            }
        }
        // Enemy
        else
        {
            GameObject player = Method.GetPlayerObject();
            if (player != null)
            {
                _target = player.transform;
            }
        }
    }

    void Update()
    {
        _state = StateController.CurrentEnum; // Inspector

        if (StateController.CurrentEnum == StateEnum.Idle)
        {
            if (!Values.ReturnToInitialPosition)
            {
                MoveDirection = Move.Direction.None;
            }
            else
            {
                _targetPos = _initialPos;
                if (Mathf.Abs(transform.position.x - _targetPos.x) >= DISTANCE_CLOSE_CUTOFF)
                {
                    StateController.ChangeState(StateEnum.Return);
                }
            }
        }

        if (StateController.CurrentEnum == StateEnum.Chase)
        {
            _targetPos = _target == null ? _initialPos : _target.transform.position;
            MoveDirection = UpdateMoveDirection(transform.position.x, _targetPos.x);
            MoveRigidbody(Values.ChaseAcceleration);
        }

        if (StateController.CurrentEnum == StateEnum.Return)
        {
            if (Values.ReturnToInitialPosition)
            {
                MoveDirection = UpdateMoveDirection(transform.position.x, _targetPos.x);
                MoveRigidbody(Values.WalkAcceleration);
            }
            else
            {
                StateController.ChangeState(StateEnum.Idle);
            }
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

        StartChase();
    }

    public void StayChase()
    {
        if (GameState.CurrentState != GameState.Play) { return; }

        if (StateController.CurrentEnum != StateEnum.Chase)
        {
            StartChase();
        }
    }

    void StartChase()
    {
        // Enemy Boss
        if (IsEnemyBoss())
        {
            Debug.Log("Emeny Boss Start Chase!");
            _target = _enemyBossMoveTarget;
            _enemyBossMoveTarget.GetComponent<EnemyBossMoveTarget>().StartPatrol();
        }
        // Enemy
        else
        {
            _target = Method.GetPlayerObject().transform;
        }

        StateController.ChangeState(StateEnum.Chase);
    }

    public void ExitChase()
    {
        if (IsEnemyBoss()) { return; }

        _targetPos = _initialPos;

        if (Values.ReturnToInitialPosition)
            StateController.ChangeState(StateEnum.Return);
        else StateController.ChangeState(StateEnum.Idle);
    }

    bool IsEnemyBoss() => gameObject.tag == Constants.TAG[Constants.ENUM_TAG.ENEMY_BOSS];
}