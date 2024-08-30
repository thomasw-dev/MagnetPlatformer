using UnityEngine;

public class EnemyBossMoveTargetPoint : MonoBehaviour
{
    [SerializeField] EnemyBossController _enemyBossController;
    [SerializeField] EnemyBossMoveTargetTriggerArea _triggerAreaLeft;
    [SerializeField] EnemyBossMoveTargetTriggerArea _triggerAreaRight;

    [Header("Magnetic Object")]
    [SerializeField] EnemyBossMoveTargetTriggerArea _Object1L;
    [SerializeField] EnemyBossMoveTargetTriggerArea _Object1R;
    [SerializeField] EnemyBossMoveTargetTriggerArea _Object2L;
    [SerializeField] EnemyBossMoveTargetTriggerArea _Object2R;
    [SerializeField] EnemyBossMoveTargetTriggerArea _Object3L;
    [SerializeField] EnemyBossMoveTargetTriggerArea _Object3R;
    [SerializeField] EnemyBossMoveTargetTriggerArea _Object4L;
    [SerializeField] EnemyBossMoveTargetTriggerArea _Object4R;

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

        _Object1L.OnEnemyBossEnter += MoveTargetPointToLeft;
        _Object1R.OnEnemyBossEnter += MoveTargetPointToRight;
        _Object2L.OnEnemyBossEnter += MoveTargetPointToLeft;
        _Object2R.OnEnemyBossEnter += MoveTargetPointToRight;
        _Object3L.OnEnemyBossEnter += MoveTargetPointToLeft;
        _Object3R.OnEnemyBossEnter += MoveTargetPointToRight;
        _Object4L.OnEnemyBossEnter += MoveTargetPointToLeft;
        _Object4R.OnEnemyBossEnter += MoveTargetPointToRight;
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

        _Object1L.OnEnemyBossEnter -= MoveTargetPointToLeft;
        _Object1R.OnEnemyBossEnter -= MoveTargetPointToRight;
        _Object2L.OnEnemyBossEnter -= MoveTargetPointToLeft;
        _Object2R.OnEnemyBossEnter -= MoveTargetPointToRight;
        _Object3L.OnEnemyBossEnter -= MoveTargetPointToLeft;
        _Object3R.OnEnemyBossEnter -= MoveTargetPointToRight;
        _Object4L.OnEnemyBossEnter -= MoveTargetPointToLeft;
        _Object4R.OnEnemyBossEnter -= MoveTargetPointToRight;
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
