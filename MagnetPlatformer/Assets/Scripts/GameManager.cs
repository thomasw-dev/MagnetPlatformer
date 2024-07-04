using System;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    #region GAME STATES, ACTIONS

    public enum State
    {
        Initialization, Playing, Win, Lose
    }

    public static event Action OnInitializationEnter, OnInitializationExit;
    public static event Action OnPlayingEnter, OnPlayingExit;
    public static event Action OnWinEnter, OnWinExit;
    public static event Action OnLoseEnter, OnLoseExit;

    static Action GetStateEnterExitAction(State gameState, bool isEnter)
    {
        return gameState switch
        {
            State.Initialization => isEnter ? OnInitializationEnter : OnInitializationExit,
            State.Playing => isEnter ? OnPlayingEnter : OnPlayingExit,
            State.Win => isEnter ? OnWinEnter : OnWinExit,
            State.Lose => isEnter ? OnLoseEnter : OnLoseExit,
            _ => null
        };
    }

    private static State _gameState;
    public static State GameState
    {
        get { return _gameState; }

        set
        {
            GetStateEnterExitAction(_gameState, false)?.Invoke();
            _gameState = value;
            GetStateEnterExitAction(_gameState, true)?.Invoke();
        }
    }
    [SerializeField] State _state;

    #endregion

    // Subscribe to the OnGoalAchieved event in these GameObjects
    [SerializeField] List<GoalObject> _goals;
    int _goalRemaining = 1;

    void OnEnable()
    {
        _goalRemaining = _goals.Count;
        for (int i = 0; i < _goals.Count; i++)
        {
            _goals[i].OnGoalAchieved += AddGoalProgress;
        }
    }

    void OnDisable()
    {

    }

    void Start()
    {
        SetState(State.Playing);
    }

    void SetState(State gameState)
    {
        GameState = gameState;
        Debug.Log($"Game State: {GameState}");
    }

    void AddGoalProgress()
    {
        _goalRemaining--;
        if (_goalRemaining == 0)
        {
            SetState(State.Win);
        }
    }
}
