using UnityEngine;

public class EnemyVisual : MonoBehaviour
{
    [Header("Dependencies")] // Required to be assigned in the Inspector
    [SerializeField] Animator _animator;

    EnemyController _enemyController;

    void Awake()
    {
        _enemyController = transform.parent.GetComponent<EnemyController>();
    }

    void OnEnable()
    {
        _enemyController.StateController.EnumToState(Enemy.StateEnum.Idle).OnEnter += Idle;
        _enemyController.OnMoveDirectionChange += ChangeMoveDirection;
    }

    void OnDisable()
    {
        _enemyController.StateController.EnumToState(Enemy.StateEnum.Idle).OnEnter -= Idle;
        _enemyController.OnMoveDirectionChange -= ChangeMoveDirection;
    }

    void Idle() => _animator.SetTrigger("Idle");

    void ChangeMoveDirection(Move.Direction direction)
    {
        if (direction == Move.Direction.None) _animator.SetTrigger("Idle");
        if (direction == Move.Direction.Left) _animator.SetTrigger("MoveLeft");
        if (direction == Move.Direction.Right) _animator.SetTrigger("MoveRight");
    }
}