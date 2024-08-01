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
        _enemyController.StateController.OnCurrentStateChanged += UpdateMoveDirectionOnStateChange;
        _enemyController.OnMoveDirectionChange += ChangeMoveDirection;
    }

    void OnDisable()
    {
        _enemyController.StateController.OnCurrentStateChanged -= UpdateMoveDirectionOnStateChange;
        _enemyController.OnMoveDirectionChange -= ChangeMoveDirection;
    }

    void UpdateMoveDirectionOnStateChange()
    {
        Debug.Log("UpdateMoveDirectionOnStateChange");
        ChangeMoveDirection(_enemyController.MoveDirection);
    }

    void ChangeMoveDirection(Move.Direction direction)
    {
        EnemyController.StateEnum controllerState = _enemyController.StateController.CurrentEnum;

        if (direction == Move.Direction.None)
        {
            _animator.SetTrigger("Idle");
        }
        if (direction == Move.Direction.Left)
        {
            if (controllerState == EnemyController.StateEnum.Chase) _animator.SetTrigger("MoveLeft");
            if (controllerState == EnemyController.StateEnum.Return) _animator.SetTrigger("WalkLeft");
        }
        if (direction == Move.Direction.Right)
        {
            if (controllerState == EnemyController.StateEnum.Chase) _animator.SetTrigger("MoveRight");
            if (controllerState == EnemyController.StateEnum.Return) _animator.SetTrigger("WalkRight");
        }
    }
}