using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    // GOALS

    [SerializeField] int _goalRemaining = 1;
    [SerializeField] List<GoalObject> _goals;

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
            if (Mod.EnableWin)
            {
                GameState.ChangeState(GameState.Win);
            }
            else
            {
                Debug.Log("Win is detected, but disabled by mods.");
            }
        }
    }

    void Death()
    {
        if (GameState.CurrentState != GameState.Play) { return; }

        if (Mod.EnableDeath)
        {
            GameState.ChangeState(GameState.Lose);
        }
        else
        {
            Debug.Log("Lose is detected, but disabled by mods.");
        }
    }
}
