using UnityEngine;

public class EnemyVisual : MonoBehaviour
{
    [SerializeField] Enemy _enemy;
    Animator _animator;

    void Awake()
    {
        _animator = GetComponent<Animator>();
    }

    void OnEnable()
    {
        _enemy.StateController.EnumToState(Enemy.StateEnum.Idle).OnEnter += Idle;
        _enemy.OnMoveDirectionChange += ChangeMoveDirection;
    }

    void OnDisable()
    {
        _enemy.StateController.EnumToState(Enemy.StateEnum.Idle).OnEnter -= Idle;
        _enemy.OnMoveDirectionChange -= ChangeMoveDirection;
    }

    void Idle() => _animator.SetTrigger("Idle");

    void ChangeMoveDirection(Move.Direction direction)
    {
        if (direction == Move.Direction.None) _animator.SetTrigger("Idle");
        if (direction == Move.Direction.Left) _animator.SetTrigger("MoveLeft");
        if (direction == Move.Direction.Right) _animator.SetTrigger("MoveRight");
    }
}