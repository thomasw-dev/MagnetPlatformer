using UnityEngine;

public class EnemyBossDeathTrigger : MonoBehaviour
{
    EnemyBossController _enemyBossController;

    void Awake()
    {
        _enemyBossController = transform.parent.GetComponent<EnemyBossController>();
    }

    void OnTriggerEnter2D(Collider2D col)
    {
        if (Method.IsPlayerObject(col.gameObject))
        {
            if (Mod.EnableDeath)
            {
                _enemyBossController.OnKillPlayer?.Invoke();
                GameEvent.Raise(GameEvent.Event.Death);
            }
        }
    }
}
