using System;
using UnityEngine;

[Serializable]
public abstract class Formula : ScriptableObject
{
    public abstract float GetValue(float input);
}