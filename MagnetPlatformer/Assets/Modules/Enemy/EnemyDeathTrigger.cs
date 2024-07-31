using UnityEngine;

public class EnemyDeathTrigger : MonoBehaviour
{
    EnemyController _enemyController;
    EnemyValues _enemyValues;
    CircleCollider2D _circleCollider;

    void Awake()
    {
        _enemyController = transform.parent.GetComponent<EnemyController>();
        _enemyValues = transform.parent.GetComponent<EnemyValues>();
    }

    void Update()
    {
        _circleCollider.radius = _enemyValues.ChaseRadius;
    }

    void OnTriggerEnter2D(Collider2D col)
    {
        if (Method.IsPlayerObject(col.gameObject))
        {
            if (Mod.EnableDeath)
            {
                _enemyController.OnKillPlayer?.Invoke();
                GameEvent.Raise(GameEvent.Event.Death);
            }
        }
    }
}
