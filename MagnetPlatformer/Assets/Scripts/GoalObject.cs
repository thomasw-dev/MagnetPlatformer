using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// When the script is attached to a GameObject,
/// it is define as the goal (level complete on touch).
/// </summary>
public class GoalObject : MonoBehaviour
{
    public event Action OnGoalAchieved;
    event Action OnPlayerTouch;



    void OnTriggerEnter2D(Collider2D col)
    {
        if (Method.IsPlayerObject(col.gameObject))
        {
            OnPlayerTouch?.Invoke();
        }
    }
}
