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
    Color _disabledColor = new Color(0.5f, 0.5f, 0.5f, 1f);

    void OnEnable()
    {
        // When player touch this object, the goal is achieved
        OnPlayerTouch += AchieveGoal;
    }

    void OnDisable()
    {
        OnPlayerTouch -= AchieveGoal;
    }

    void OnTriggerEnter2D(Collider2D col)
    {
        if (_state == State.Disabled) { return; }

        if (Method.IsPlayerObject(col.gameObject))
        {
            OnPlayerTouch?.Invoke();
            _state = State.Disabled;
        }
    }

    void AchieveGoal()
    {
        OnGoalAchieved?.Invoke();
        SetDisabledVisual();
    }

    void SetDisabledVisual()
    {
        _spriteRenderer.color = _disabledColor;
    }
}
