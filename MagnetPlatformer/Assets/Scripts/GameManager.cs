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

    static Action GetStateEnterAction(State gameState)
    {
        return gameState switch
        {
            State.Initialization => OnInitializationEnter,
            State.Playing => OnPlayingEnter,
            State.Win => OnWinEnter,
            State.Lose => OnLoseEnter,
            _ => null
        };
    }

    static Action GetStateExitAction(State gameState)
    {
        return gameState switch
        {
            State.Initialization => OnInitializationExit,
            State.Playing => OnPlayingExit,
            State.Win => OnWinExit,
            State.Lose => OnLoseExit,
            _ => null
        };
    }

    private static State _gameState;
    public static State GameState
    {
        get { return _gameState; }

        set
        {
            GetStateExitAction(_gameState)?.Invoke();
            _gameState = value;
            GetStateEnterAction(_gameState)?.Invoke();
        }
    }
    [SerializeField] State _state; // Inspector

    #endregion

    #region GOALS

    [SerializeField] int _goalRemaining = 1;
    [SerializeField] List<GoalObject> _goals;

    #endregion

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
        _goalRemaining = 1;
        for (int i = 0; i < _goals.Count; i++)
        {
            _goals[i].OnGoalAchieved -= AddGoalProgress;
        }
    }

    void Start()
    {
        SetState(State.Playing);
    }

    void Update()
    {
        _state = GameState;
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
