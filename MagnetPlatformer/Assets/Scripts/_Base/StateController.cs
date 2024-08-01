using System;
using System.Collections.Generic;
using System.Linq;

public class StateController<T> where T : Enum
{
    public List<State> States = new List<State>();

    public StateController()
    {
        T[] array = (T[])Enum.GetValues(typeof(T));
        for (int i = 0; i < array.Length; i++)
        {
            States.Add(new State(array[i].ToString()));
        }
    }

    State _currentState;
    public State CurrentState
    {
        get => _currentState;
        set
        {
            _currentState?.Exit();
            _currentState = value;
            _currentState?.Enter();
            OnCurrentStateChanged?.Invoke();
        }
    }
    public event Action OnCurrentStateChanged;

    public void ChangeState(T stateEnum)
    {
        State state = States.First(item => item.Name == stateEnum.ToString());
        CurrentState = state;
    }

    public T CurrentEnum => (T)Enum.Parse(typeof(T), CurrentState.Name);

    public State EnumToState<E>(E stateEnum) where E : Enum
    {
        return States.First(item => item.Name == stateEnum.ToString());
    }
}