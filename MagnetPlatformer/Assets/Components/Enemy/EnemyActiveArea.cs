using System;
using UnityEngine;

public class EnemyActiveArea : MonoBehaviour
{
    public event Action OnPlayerEnter;
    public event Action OnPlayerExit;

    void OnTriggerEnter2D(Collider2D col)
    {
        if (Method.IsPlayerObject(col.gameObject))
        {
            OnPlayerEnter.Invoke();
        }
    }

    void OnTriggerExit2D(Collider2D col)
    {
        if (Method.IsPlayerObject(col.gameObject))
        {
            OnPlayerExit.Invoke();
        }
    }
}
