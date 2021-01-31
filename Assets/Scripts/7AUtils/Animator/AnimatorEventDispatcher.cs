using System;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Animator))]
public class AnimatorEventDispatcher : MonoBehaviour
{
	[SerializeField] private int m_animationEventsCount = 1;

	private Dictionary<int, Action> m_animationEvents = new Dictionary<int, Action>();

    private int eventIndex = 0;

    private void Awake()
	{

    }

	public void Bind(int index, Action callback)
	{
		m_animationEvents[index] = callback;
	}

	public void Unbind(int index, Action callback = null)
	{
		m_animationEvents[index] = null;
	}

	public void EventDispatch()
	{
		if (m_animationEvents != null && m_animationEvents[eventIndex] != null)
			m_animationEvents[eventIndex].Invoke();

		eventIndex = (eventIndex + 1) % m_animationEvents.Count;
	}

    public void EventDispatchIndex(int index)
    {
        m_animationEvents[index].Invoke();
    }
}
