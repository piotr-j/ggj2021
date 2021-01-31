using System;
using UnityEngine;
using UnityEngine.Events;

[CreateAssetMenu(fileName = "Float Event", menuName = "7A Utils/Events/Float Event"), Serializable]
public class FloatEvent : GameEvent<float>
{

}

[Serializable]
public class UnityFloatEvent : UnityEvent<float> { }