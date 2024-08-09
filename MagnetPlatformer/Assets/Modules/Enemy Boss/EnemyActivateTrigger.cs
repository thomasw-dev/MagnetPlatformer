using System;
using UnityEngine;

public class EnemyActivateTrigger : MonoBehaviour
{
    public event Action OnActivate;
    bool _activated = false;

    //EnemyController GetController() => transform.parent.GetComponent<EnemyController>();

    void OnTriggerEnter2D(Collider2D col)
    {
        if (!_activated) { return; }
        if (!Method.IsPlayerObject(col.gameObject)) { return; }

        OnActivate?.Invoke();
    }

    void OnTriggerStay2D(Collider2D col)
    {
        if (!_activated) { return; }
        if (!Method.IsPlayerObject(col.gameObject)) { return; }

        OnActivate?.Invoke();
    }

    void OnTriggerExit2D(Collider2D col)
    {
        if (!_activated) { return; }
        if (!Method.IsPlayerObject(col.gameObject)) { return; }

        _activated = false;
    }
}
