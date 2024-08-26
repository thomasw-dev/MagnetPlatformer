using System;
using UnityEngine;

public class Checkpoint : PlayerTriggerEvents
{
    public event Action<Checkpoint> OnPlayerReach;

    SpriteRenderer _spriteRenderer;

    void Awake()
    {
        _spriteRenderer = GetComponent<SpriteRenderer>();
    }

    void OnEnable()
    {
        OnPlayerEnter += PlayerReachCheckpoint;
    }

    void OnDisable()
    {
        OnPlayerEnter -= PlayerReachCheckpoint;
    }

    void PlayerReachCheckpoint()
    {
        OnPlayerReach?.Invoke(this);

        if (_spriteRenderer != null)
        {
            _spriteRenderer.color = Color.white;
        }
    }
}
