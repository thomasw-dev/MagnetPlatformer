using System;
using UnityEngine;

public class EnemyRadiusArea : MonoBehaviour
{
    public event Action OnPlayerEnter;
    public event Action OnPlayerExit;

    //[SerializeField] bool _gizmos = true;

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

    public void DrawGizmos(float radius)
    {
        Gizmos.color = Color.magenta;
        Gizmos.DrawWireSphere(transform.position, radius);
    }
}
