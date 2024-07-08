using System;
using UnityEngine;

public class CameraStickyArea : MonoBehaviour
{
    public static event Action<Vector3> OnTriggerEnter;
    public static event Action OnTriggerExit;

    void OnTriggerEnter2D(Collider2D col)
    {
        if (Method.IsPlayerObject(col.gameObject))
        {
            OnTriggerEnter?.Invoke(transform.position);
        }
    }

    void OnTriggerExit2D(Collider2D col)
    {
        if (Method.IsPlayerObject(col.gameObject))
        {
            OnTriggerExit?.Invoke();
        }
    }
}
