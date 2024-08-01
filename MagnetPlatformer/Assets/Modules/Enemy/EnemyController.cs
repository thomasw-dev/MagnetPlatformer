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

    GameObject _target;
    const float DISTANCE_CLOSE_CUTOFF = 0.1f;

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
    }

    void Update()
    {
        _state = StateController.CurrentEnum; // Inspector

        if (StateController.CurrentEnum == StateEnum.Idle)
        {
            Idle();
        }

        if (StateController.CurrentEnum == StateEnum.Chase)
        {
            Chase();
        }

        if (StateController.CurrentEnum == StateEnum.Return)
        {
            ReturnToInitialPosition();
        }
    }

    void Idle()
    {
        MoveDirection = Move.Direction.None;
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

    void Chase()
    {
        MoveDirection = UpdateMoveDirection(transform.position.x, _target.transform.position.x);

        Vector2 moveForce = Vector2.zero;
        if (MoveDirection == Move.Direction.None) moveForce = Vector2.zero;
        if (MoveDirection == Move.Direction.Left) moveForce = Vector2.left * Values.Acceleration;
        if (MoveDirection == Move.Direction.Right) moveForce = Vector2.right * Values.Acceleration;
        _rigidbody.AddForce(moveForce);
    }

    void ReturnToInitialPosition()
    {
        Debug.Log("Return...");
    }

    public void EnterChase()
    {
        if (GameState.CurrentState != GameState.Play) { return; }

        _target = Method.GetPlayerObject();
        StateController.ChangeState(StateEnum.Chase);
    }

    public void StayChase()
    {
        if (GameState.CurrentState != GameState.Play) { return; }

        if (StateController.CurrentEnum != StateEnum.Chase)
        {
            _target = Method.GetPlayerObject();
            StateController.ChangeState(StateEnum.Chase);
        }
    }

    public void ExitChase()
    {
        if (Values.ReturnToInitialPosition)
        {
            _target = Method.GetPlayerObject();
            StateController.ChangeState(StateEnum.Return);
        }
        else
        {
            StateController.ChangeState(StateEnum.Idle);
        }
    }
}