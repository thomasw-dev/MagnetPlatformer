using System;
using UnityEngine;

public class EnemyBossMoveTargetTriggerArea : MonoBehaviour
{
    public event Action OnEnemyBossEnter;

    void OnTriggerEnter2D(Collider2D col)
    {
        if (col.gameObject.tag == Constants.TAG[Constants.ENUM_TAG.ENEMY_BOSS])
        {
            OnEnemyBossEnter?.Invoke();
        }
    }
}
