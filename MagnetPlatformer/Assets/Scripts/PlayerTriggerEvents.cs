using System;
using UnityEngine;

public class PlayerTriggerEvents : MonoBehaviour
{
    public event Action OnPlayerEnter;
    public event Action OnPlayerStay;
    public event Action OnPlayerExit;

    void OnTriggerEnter2D(Collider2D col)
    {
        if (Method.IsPlayerObject(col.gameObject))
        {
            OnPlayerEnter?.Invoke();
        }
    }

    void OnTriggerStay2D(Collider2D col)
    {
        if (Method.IsPlayerObject(col.gameObject))
        {
            OnPlayerStay?.Invoke();
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
