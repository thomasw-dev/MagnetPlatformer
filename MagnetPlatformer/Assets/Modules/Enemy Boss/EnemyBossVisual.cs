using UnityEngine;

public class EnemyBossVisual : MonoBehaviour
{
    [Header("Dependencies")] // Required to be assigned in the Inspector
    [SerializeField] Animator _animator;

    EnemyBossController _enemyBossController;

    protected enum AnimatorState { Idle, MoveLeft, MoveRight, WalkLeft, WalkRight }
    AnimatorState _animatorState = AnimatorState.Idle;

    void Awake()
    {
        _enemyBossController = transform.parent.GetComponent<EnemyBossController>();
    }

    void Update()
    {
        EnemyBossController.StateEnum currentState = _enemyBossController.StateController.CurrentEnum;
        Move.Direction moveDirection = _enemyBossController.MoveDirection;

        if (currentState == EnemyBossController.StateEnum.Idle || moveDirection == Move.Direction.None)
        {
            if (_animatorState != AnimatorState.Idle)
            {
                _animator.SetTrigger("Idle");
                _animatorState = AnimatorState.Idle;
            }
        }
        if (currentState == EnemyBossController.StateEnum.Chase && moveDirection == Move.Direction.Left)
        {
            if (_animatorState != AnimatorState.MoveLeft)
            {
                _animator.SetTrigger("MoveLeft");
                _animatorState = AnimatorState.MoveLeft;
            }
        }
        if (currentState == EnemyBossController.StateEnum.Chase && moveDirection == Move.Direction.Right)
        {
            if (_animatorState != AnimatorState.MoveRight)
            {
                _animator.SetTrigger("MoveRight");
                _animatorState = AnimatorState.MoveRight;
            }
        }
    }
}