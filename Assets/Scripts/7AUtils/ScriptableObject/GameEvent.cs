// ----------------------------------------------------------------------------
// Unite 2017 - Game Architecture with Scriptable Objects
// 
// Author: Ryan Hipple
// Date:   10/04/17
// ----------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Generic Event", menuName = "7A Utils/Events/Generic Event")]
public class GameEvent : ScriptableObject
{
    /// <summary>
    /// The list of listeners that this event will notify if it is raised.
    /// </summary>
    private readonly List<GameEventListener> eventListeners = 
        new List<GameEventListener>();

    public Action action;

    public bool debugLog;

    public void Raise()
    {
        for(int i = eventListeners.Count -1; i >= 0; i--)
            eventListeners[i].OnEventRaised();

        action?.Invoke();

        // ReSharper disable once Unity.PerformanceCriticalCodeInvocation
        if (debugLog) Debug.Log(name + " game event raised", this);
    }

    public void RegisterListener(GameEventListener listener)
    {
        if (!eventListeners.Contains(listener))
            eventListeners.Add(listener);
    }

    public void UnregisterListener(GameEventListener listener)
    {
        if (eventListeners.Contains(listener))
            eventListeners.Remove(listener);
    }

    public void RegisterAction(Action callback)
    {
        action += callback;
    }

    public void UnregisterAction(Action callback)
    {
        action -= callback;
    }
}

public class GameEvent<T> : ScriptableObject
{
    /// <summary>
    /// The list of listeners that this event will notify if it is raised.
    /// </summary>
    private readonly List<IGameEventListener<T>> eventListeners =
        new List<IGameEventListener<T>>();

    public Action<T> action;

    public bool debugLog;

    public T testRaiseData;

    public void Raise(T obj)
    {
        for (int i = eventListeners.Count - 1; i >= 0; i--)
            eventListeners[i].OnEventRaised(obj);

        action?.Invoke(obj);

        if (debugLog) Debug.LogWarning(name + " game event raised " + obj, this);
    }

    public void RegisterListener(IGameEventListener<T> listener)
    {
        if (!eventListeners.Contains(listener))
            eventListeners.Add(listener);
    }

    public void UnregisterListener(IGameEventListener<T> listener)
    {
        if (eventListeners.Contains(listener))
            eventListeners.Remove(listener);
    }

    public void RegisterAction(Action<T> callback)
    {
        action += callback;
    }

    public void UnregisterAction(Action<T> callback)
    {
        action -= callback;
    }
}