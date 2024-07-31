using System;
using UnityEngine;

public class EnemyChaseTrigger : MonoBehaviour
{
    public event Action OnPlayerEnter;
    public event Action OnPlayerExit;

    EnemyController _enemyController;

    void Awake()
    {
        _enemyController = transform.parent.GetComponent<EnemyController>();
    }

    void OnEnable()
    {
        OnPlayerEnter += _enemyController.StartChase;
        OnPlayerExit += _enemyController.StopChase;
    }

    void OnDisable()
    {
        OnPlayerEnter -= _enemyController.StartChase;
        OnPlayerExit -= _enemyController.StopChase;
    }

    void OnTriggerEnter2D(Collider2D col)
    {
        if (Method.IsPlayerObject(col.gameObject))
        {
            OnPlayerEnter?.Invoke();
        }
    }

    void OnTriggerExit2D(Collider2D col)
    {
        if (Method.IsPlayerObject(col.gameObject))
        {
            OnPlayerExit?.Invoke();
        }
    }
}
