using UnityEngine;

public class Death : MonoBehaviour
{
    void OnTriggerEnter2D(Collider2D col)
    {
        if (Method.IsPlayerObject(col.gameObject))
        {
            GameEvent.Raise(GameEvent.Event.Death);
        }
    }
}
