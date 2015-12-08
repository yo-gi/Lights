using System;
using System.Collections.Generic;

public class OnResetEvent { }
public class OnTorchLitEvent { public Torch torch; }
public class OnTorchUnlitEvent { public Torch torch; }
public class OnTorchGroupLitEvent { public TorchGroup group; }
public class OnAltarLitEvent { public Ability ability; }
public class OnDeathEvent { }
public class OnPauseEvent { public bool paused; }

public static class Events
{
    private static EventStore handler = new EventStore();

    // Use this method to listen for an event. For example:
    //
    //  Events.Register<EventTypeHere>(() => {
    //    Debug.Log("EventTypeHere happened!");
    //  });
    //
    public static void Register<T>(Action handler)
    {
        Events.handler.Register<T>(handler);
    }

    // Use this method to listen for an event. For example:
    //
    //  Events.Register<EventTypeHere>((e) => {
    //    Debug.Log("Event " + e.someField + " happened!");
    //  });
    //
    public static void Register<T>(Action<T> handler)
    {
        Events.handler.Register<T>(handler);
    }

    // Use this kick off an event. For example:
    //
    //  Events.Broadcast(new EventTypeHere() {
    //    someField = "someValue"
    //  });
    //
    public static void Broadcast<T>(T e)
    {
        Events.handler.Broadcast<T>(e);
    }
}

public class EventStore
{
    private readonly Dictionary<Type, List<object>> handlers = new Dictionary<Type, List<object>>();

    public void Register<T>(Action handler)
    {
        var type = typeof(T);
        if (handlers.ContainsKey(type) == false)
        {
            handlers[type] = new List<object>();
        }
        handlers[type].Add(handler);
    }

    public void Register<T>(Action<T> handler)
    {
        var type = typeof(T);
        if (handlers.ContainsKey(type) == false)
        {
            handlers[type] = new List<object>();
        }
        handlers[type].Add(handler);
    }

    public void Broadcast<T>(T e)
    {
        var type = typeof(T);
        if (handlers.ContainsKey(type) == false) return;
        foreach (var callback in handlers[type])
        {
            var parameterlessHandler = callback as Action;
            if (parameterlessHandler == null)
            {
                var handler = (Action<T>)callback;
                handler(e);
            }
            else
            {
                parameterlessHandler();
            }
        }
    }
}