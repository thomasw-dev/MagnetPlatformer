using System;
using UnityEngine;

public class EnemyChaseTrigger : MonoBehaviour
{
    public event Action OnPlayerEnter;
    public event Action OnPlayerStay;
    public event Action OnPlayerExit;

    EnemyController GetController() => transform.parent.GetComponent<EnemyController>();

    void OnEnable()
    {
        OnPlayerEnter += GetController().EnterChase;
        OnPlayerStay += GetController().StayChase;
        OnPlayerExit += GetController().ExitChase;
    }

    void OnDisable()
    {
        OnPlayerEnter -= GetController().EnterChase;
        OnPlayerStay -= GetController().StayChase;
        OnPlayerExit -= GetController().ExitChase;
    }

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
