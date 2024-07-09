using System;

public class State
{
    public State(string name)
    {
        Name = name;
    }
    public string Name { get; }

    public event Action OnEnter;
    public event Action OnExit;

    public void Enter() => OnEnter?.Invoke();
    public void Exit() => OnExit?.Invoke();
}