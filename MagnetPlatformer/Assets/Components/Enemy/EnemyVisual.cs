using UnityEngine;

public class EnemyVisual : MonoBehaviour
{
    Enemy _enemy;
    Animator _animator;

    void Awake()
    {
        _enemy = Method.GetParentsMeetCondition(gameObject, HasEnabledEnemy)[0].GetComponent<Enemy>();
        bool HasEnabledEnemy(GameObject gameObject) => Method.HasEnabledComponent<Enemy>(gameObject);
    }

    void OnEnable()
    {
        _enemy.StateController.EnumToState(Enemy.EnumState.Idle).OnEnter += Idle;
    }

    void OnDisable()
    {
        _enemy.StateController.EnumToState(Enemy.EnumState.Idle).OnEnter -= Idle;
    }

    void Update()
    {
        if (_enemy.StateController.CurrentEnum == Enemy.EnumState.Chase)
        {
            // Move target pos is on the left
            if (_enemy.transform.position.x > _enemy.MoveTargetPos.x)
            {
                _animator.SetTrigger("MoveLeft");
            }
            // Move target pos is on the right
            if (_enemy.transform.position.x < _enemy.MoveTargetPos.x)
            {
                _animator.SetTrigger("MoveRight");
            }
        }
    }

    void Idle() => _animator.SetTrigger("Idle");
}
