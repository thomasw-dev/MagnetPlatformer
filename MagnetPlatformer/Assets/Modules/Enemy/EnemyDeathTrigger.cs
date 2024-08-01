using UnityEngine;

public class EnemyDeathTrigger : MonoBehaviour
{
    EnemyController _enemyController;

    void Awake()
    {
        _enemyController = transform.parent.GetComponent<EnemyController>();
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
