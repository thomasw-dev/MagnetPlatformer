using UnityEngine;

public class EnemyBossMoveTargetPoint : MonoBehaviour
{
    [SerializeField] EnemyBossMoveTargetTriggerArea _triggerAreaLeft;
    [SerializeField] EnemyBossMoveTargetTriggerArea _triggerAreaRight;

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

    void MoveTargetPointToLeft()
    {
        transform.position = _triggerAreaLeft.transform.position;
    }

    void MoveTargetPointToRight()
    {
        transform.position = _triggerAreaRight.transform.position;
    }
}
