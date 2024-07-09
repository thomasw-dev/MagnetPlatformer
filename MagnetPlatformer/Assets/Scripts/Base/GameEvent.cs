using System;
using System.Collections.Generic;

public static class GameEvent
{
    public enum Events
    {
        PlayerCollide,
        PlayerTouchTrap
    }

    public static Dictionary<Events, Action> doOnEvent = new();

    public static void Subscribe(Events e, Action handler)
    {
        if (!doOnEvent.ContainsKey(e)) { doOnEvent[e] = null; }

        doOnEvent[e] += handler;
    }

    public static void Unsubscribe(Events e, Action handler)
    {
        if (!doOnEvent.ContainsKey(e)) { return; }

        doOnEvent[e] -= handler;
    }

    public static void Trigger(Events e)
    {
        doOnEvent[e]?.Invoke();
    }
}
