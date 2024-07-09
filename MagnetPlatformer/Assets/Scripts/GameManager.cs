using System;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    #region GOALS

    [SerializeField] int _goalRemaining = 1;
    [SerializeField] List<GoalObject> _goals;

    #endregion

    void OnEnable()
    {
        GameEvent.Subscribe(GameEvent.Event.Death, Death);

        ClickToStart.OnClick += StartGame;
        InputManager.OnAnyKeyInput += StartGame;

        _goalRemaining = _goals.Count;
        for (int i = 0; i < _goals.Count; i++)
        {
            _goals[i].OnGoalAchieved += AddGoalProgress;
        }
    }

    void OnDisable()
    {
        GameEvent.Unsubscribe(GameEvent.Event.Death, Death);

        ClickToStart.OnClick -= StartGame;
        InputManager.OnAnyKeyInput -= StartGame;

        _goalRemaining = 1;
        for (int i = 0; i < _goals.Count; i++)
        {
            _goals[i].OnGoalAchieved -= AddGoalProgress;
        }
    }

    void Start()
    {
        GameState.ChangeState(GameState.Initialize);
    }

    void StartGame() => GameState.ChangeState(GameState.Play);

    void AddGoalProgress()
    {
        _goalRemaining--;
        if (_goalRemaining == 0)
        {
            GameState.ChangeState(GameState.Win);
        }
    }

    void Death() => GameState.ChangeState(GameState.Lose);
}
