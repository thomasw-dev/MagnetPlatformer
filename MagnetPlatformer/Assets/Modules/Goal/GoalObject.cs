using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Define the GameObject it is attached to as a goal.
/// </summary>
public class GoalObject : MonoBehaviour
{
    enum State { Enabled, Disabled }
    State _state = State.Enabled;

    public event Action OnGoalAchieved;
    event Action OnPlayerTouch;

    [SerializeField] SpriteRenderer _spriteRenderer;
    [SerializeField] Sprite _disabledSprite;

    Rigidbody2D _rigidbody;

    void Awake()
    {
        _rigidbody = GetComponent<Rigidbody2D>();
    }

    void OnEnable()
    {
        GameState.Initialize.OnEnter += Initialize;

        // When player touch this object, the goal is achieved
        OnPlayerTouch += AchieveGoal;
    }

    void OnDisable()
    {
        GameState.Initialize.OnEnter -= Initialize;
        OnPlayerTouch -= AchieveGoal;
    }

    void Initialize()
    {
    }

    //void EnablePhysics() => _rigidbody.bodyType = RigidbodyType2D.Dynamic;

    void OnTriggerEnter2D(Collider2D col)
    {
        if (_state == State.Disabled) { return; }

        if (Method.IsPlayerObject(col.gameObject))
        {
            OnPlayerTouch?.Invoke();
        }
    }

    void AchieveGoal()
    {
        _state = State.Disabled;
        OnGoalAchieved?.Invoke();
        SetDisabledVisual();
    }

    void SetDisabledVisual()
    {
        _spriteRenderer.sprite = _disabledSprite;
    }
}
