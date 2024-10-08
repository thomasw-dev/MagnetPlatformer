using UnityEngine;

public class DeathTrigger : MonoBehaviour
{
    void OnTriggerEnter2D(Collider2D col)
    {
        if (Method.IsPlayerObject(col.gameObject))
        {
            if (Mod.EnableDeath)
            {
                GameEvent.Raise(GameEvent.Event.Death);
            }
        }
    }
}
