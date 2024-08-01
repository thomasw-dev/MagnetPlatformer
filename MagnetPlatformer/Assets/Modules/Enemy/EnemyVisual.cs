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

    void Update()
    {
        EnemyController.StateEnum currentState = _enemyController.StateController.CurrentEnum;
        Move.Direction moveDirection = _enemyController.MoveDirection;

        _animator.SetBool("Idle", currentState == EnemyController.StateEnum.Idle || moveDirection == Move.Direction.None);
        _animator.SetBool("MoveLeft", currentState == EnemyController.StateEnum.Chase && moveDirection == Move.Direction.Left);
        _animator.SetBool("MoveRight", currentState == EnemyController.StateEnum.Chase && moveDirection == Move.Direction.Right);
        _animator.SetBool("WalkLeft", currentState == EnemyController.StateEnum.Return && moveDirection == Move.Direction.Left);
        _animator.SetBool("WalkRight", currentState == EnemyController.StateEnum.Return && moveDirection == Move.Direction.Right);
    }
}