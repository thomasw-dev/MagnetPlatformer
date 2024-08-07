using System;
using UnityEngine;

public class EnemyActiveArea : MonoBehaviour
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

    public void DrawGizmos()
    {
        BoxCollider2D boxCollider = GetComponent<BoxCollider2D>();
        Gizmos.color = Color.magenta;
        Gizmos.DrawWireCube((Vector2)transform.position + boxCollider.offset, boxCollider.size);
    }
}
