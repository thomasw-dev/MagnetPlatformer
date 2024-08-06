using UnityEngine;

public class PlayerCollisionKill : MonoBehaviour
{
    CollisionKill _collisionKill;

    void Awake()
    {
        _collisionKill = GetComponent<CollisionKill>();
    }

    void OnEnable()
    {
        _collisionKill.OnKill += HandleKill;
    }

    void OnDisable()
    {
        _collisionKill.OnKill -= HandleKill;
    }

    void HandleKill()
    {
        GameEvent.Raise(GameEvent.Event.Death);
    }
}
