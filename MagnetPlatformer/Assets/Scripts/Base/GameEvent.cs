using System;
using System.Collections.Generic;

public static class GameEvent
{
    public enum Event
    {
        Death
    }

    public static event Action Death;

    public static Dictionary<Event, Action> EventDictionary = new Dictionary<Event, Action>
    {
        [Event.Death] = Death
    };

    public static void Subscribe(Event eventEnum, Action handler)
    {
        if (!EventDictionary.ContainsKey(eventEnum)) { return; }
        EventDictionary[eventEnum] += handler;
    }

    public static void Unsubscribe(Event eventEnum, Action handler)
    {
        if (!EventDictionary.ContainsKey(eventEnum)) { return; }
        EventDictionary[eventEnum] -= handler;
    }

    public static void Raise(Event eventEnum)
    {
        EventDictionary[eventEnum]?.Invoke();
    }
}
