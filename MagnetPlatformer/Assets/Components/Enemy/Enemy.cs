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

    void OnEnable()
    {
        _activeArea.OnPlayerEnter += Chase;
        _activeArea.OnPlayerExit += Idle;
    }

    void OnDisable()
    {
        _activeArea.OnPlayerEnter -= Chase;
        _activeArea.OnPlayerExit -= Idle;
    }

    void Start()
    {
        StateController.ChangeState(StateEnum.Idle);
    }

    void Update()
    {
        _state = StateController.CurrentEnum; // Inspector

        MoveTargetX = _player.transform.position.x;
        MoveDirection = UpdateMoveDirection(transform.position.x, MoveTargetX);

        if (StateController.CurrentEnum == StateEnum.Chase)
        {
            Vector2 move = Vector2.zero;
            if (MoveDirection == Move.Direction.Left) move = Vector2.left;
            if (MoveDirection == Move.Direction.Right) move = Vector2.right;
            transform.Translate(move * Time.deltaTime * _moveSpeed);
        }
    }

    void Idle()
    {
        if (GameState.CurrentState != GameState.Play) { return; }

        StateController.ChangeState(StateEnum.Idle);
    }

    void Chase()
    {
        if (GameState.CurrentState != GameState.Play) { return; }

        StateController.ChangeState(StateEnum.Chase);
        OnMoveDirectionChange.Invoke(MoveDirection);
    }

    Move.Direction UpdateMoveDirection(float selfX, float targetX)
    {
        if (selfX >= targetX)
            return Move.Direction.Left;
        else return Move.Direction.Right;
    }
}
