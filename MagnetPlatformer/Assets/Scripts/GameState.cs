using UnityEngine;

public class GameState : StateController
{
    public static State Initialize = new State("Initialize");
    public static State Play = new State("Play");
    public static State Win = new State("Win");
    public static State Lose = new State("Lose");

    static State _currentState;
    public static State CurrentState
    {
        get { return _currentState; }
        set
        {
            _currentState?.Exit();
            _currentState = value;
            _currentState?.Enter();
        }
    }

    public static void ChangeState(State state)
    {
        CurrentState = state;

        if (Log.GameState)
        {
            Debug.Log($"Current State: {state.Name}");
        }
    }
}