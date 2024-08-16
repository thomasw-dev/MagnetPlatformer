using UnityEngine;

public class EnemyBossMoveTargetPoint : MonoBehaviour
{
    [SerializeField] EnemyBossController _enemyBossController;
    [SerializeField] EnemyBossMoveTargetTriggerArea _triggerAreaLeft;
    [SerializeField] EnemyBossMoveTargetTriggerArea _triggerAreaRight;

    [Header("Anti-stuck")]
    [SerializeField] CollisionKill _collisionKill;

    void OnEnable()
    {
        if (_triggerAreaLeft != null)
        {
            _triggerAreaLeft.OnEnemyBossEnter += MoveTargetPointToRight;
        }
        if (_triggerAreaRight != null)
        {
            _triggerAreaRight.OnEnemyBossEnter += MoveTargetPointToLeft;
        }
    }

    void OnDisable()
    {
        if (_triggerAreaLeft != null)
        {
            _triggerAreaLeft.OnEnemyBossEnter -= MoveTargetPointToRight;
        }
        if (_triggerAreaRight != null)
        {
            _triggerAreaRight.OnEnemyBossEnter -= MoveTargetPointToLeft;
        }
    }

    void Start()
    {
        MoveTargetPointToLeft();
    }

    void Update()
    {
        if (_collisionKill != null)
        {
            FlipMoveTargetOnCollision();
        }
    }

    void FlipMoveTargetOnCollision()
    {
        switch (_enemyBossController.MoveDirection)
        {
            // If enemy boss is moving to the left and colliding with an object on the left side
            case Move.Direction.Left:
                if (_collisionKill.CollidingObjects[(int)Direction.Side.Left] != null)
                {
                    MoveTargetPointToRight();
                }
                break;

            // If enemy boss is moving to the right and colliding with an object on the right side
            case Move.Direction.Right:
                if (_collisionKill.CollidingObjects[(int)Direction.Side.Right] != null)
                {
                    MoveTargetPointToLeft();
                }
                break;
        }
    }

    void MoveTargetPointToLeft()
    {
        transform.position = _triggerAreaLeft.transform.position;
        _enemyBossController.CheckMoveDirection();
    }

    void MoveTargetPointToRight()
    {
        transform.position = _triggerAreaRight.transform.position;
        _enemyBossController.CheckMoveDirection();
    }
}
