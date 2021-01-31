// ----------------------------------------------------------------------------
// Unite 2017 - Game Architecture with Scriptable Objects
// 
// Author: Ryan Hipple
// Date:   10/04/17
// ----------------------------------------------------------------------------

using System;
using UnityEngine;
using UnityEngine.Events;

public class GameEventListener : MonoBehaviour
{
    [Tooltip("Event to register with.")]
    public GameEvent Event;

    [Tooltip("Response to invoke when Event is raised.")]
    public UnityEvent Response;

    private void OnEnable()
    {
        Event.RegisterListener(this);
    }

    private void OnDisable()
    {
        Event.UnregisterListener(this);
    }

    public void OnEventRaised()
    {
        Response.Invoke();
    }
}

[Serializable]
public class GameEventListener<T, GE> : MonoBehaviour, IGameEventListener<T> where GE : GameEvent<T>
{
    [Tooltip("Event to register with.")]
    public GE Event;
    
    [Tooltip("Response to invoke when Event is raised.")]
    public UnityEvent<T> Response;

    private void OnEnable()
    {
        Event.RegisterListener(this);
    }

    private void OnDisable()
    {
        Event.UnregisterListener(this);
    }

    public void OnEventRaised(T obj)
    {
        Response.Invoke(obj);
    }

}

public interface IGameEventListener<T>
{
    void OnEventRaised(T item);
}

public interface IGameEventListener<T1, T2>
{
    void OnEventRaised(T1 first, T2 second);
}