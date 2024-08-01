using System;
using UnityEngine;

public class EnemyChaseTrigger : MonoBehaviour
{
    public event Action OnPlayerEnter;
    public event Action OnPlayerStay;
    public event Action OnPlayerExit;

    EnemyController GetController() => transform.parent.GetComponent<EnemyController>();

    EnemyValues GetValues() => transform.parent.GetComponent<EnemyValues>();

    CircleCollider2D GetCollider() => GetComponent<CircleCollider2D>();

    void OnEnable()
    {
        OnPlayerEnter += GetController().EnterChase;
        OnPlayerStay += GetController().StayChase;
        OnPlayerExit += GetController().ExitChase;
        GetValues().SetChaseRadius += SetRadius;
    }

    void OnDisable()
    {
        OnPlayerEnter -= GetController().EnterChase;
        OnPlayerStay -= GetController().StayChase;
        OnPlayerExit -= GetController().ExitChase;
        GetValues().SetChaseRadius -= SetRadius;
    }

    void SetRadius(float radius)
    {
        GetCollider().radius = radius / 2; // #%
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
