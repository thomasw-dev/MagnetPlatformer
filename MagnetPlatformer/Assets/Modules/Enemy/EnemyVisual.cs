using UnityEngine;

public class EnemyVisual : MonoBehaviour
{
    [Header("Dependencies")] // Required to be assigned in the Inspector
    [SerializeField] Animator _animator;

    EnemyController _enemyController;

    protected enum AnimatorState { Idle, MoveLeft, MoveRight, WalkLeft, WalkRight }
    AnimatorState _animatorState = AnimatorState.Idle;

    void Awake()
    {
        _enemyController = transform.parent.GetComponent<EnemyController>();
    }

    void Update()
{
    EnemyController.StateEnum currentState = _enemyController.StateController.CurrentEnum;
    Move.Direction moveDirection = _enemyController.MoveDirection;

        if (currentState == EnemyController.StateEnum.Idle || moveDirection == Move.Direction.None)
        {
            if (_animatorState != AnimatorState.Idle)
            {
                _animator.SetTrigger("Idle");
                _animatorState = AnimatorState.Idle;
            }
        }
        if (currentState == EnemyController.StateEnum.Chase && moveDirection == Move.Direction.Left)
        {
            if (_animatorState != AnimatorState.MoveLeft)
            {
                _animator.SetTrigger("MoveLeft");
                _animatorState = AnimatorState.MoveLeft;
            }
        }
        if (currentState == EnemyController.StateEnum.Chase && moveDirection == Move.Direction.Right)
        {
            if (_animatorState != AnimatorState.MoveRight)
            {
                _animator.SetTrigger("MoveRight");
                _animatorState = AnimatorState.MoveRight;
            }
        }
        if (currentState == EnemyController.StateEnum.Return && moveDirection == Move.Direction.Left)
        {
            if (_animatorState != AnimatorState.WalkLeft)
            {
                _animator.SetTrigger("WalkLeft");
                _animatorState = AnimatorState.WalkLeft;
            }
        }
        if (currentState == EnemyController.StateEnum.Return && moveDirection == Move.Direction.Right)
        {
            if (_animatorState != AnimatorState.WalkRight)
            {
                _animator.SetTrigger("WalkRight");
                _animatorState = AnimatorState.WalkRight;
            }
        }
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
        //Debug.Log("UpdateMoveDirectionOnStateChange");
        ChangeMoveDirection(_enemyController.MoveDirection);
    }

    void ChangeMoveDirection(Move.Direction direction)
    {
        /*EnemyController.StateEnum controllerState = _enemyController.StateController.CurrentEnum;

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
        }*/
    }
}