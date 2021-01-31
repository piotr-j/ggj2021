using System;
using UnityEngine;

[CreateAssetMenu(fileName = "Base Formula", menuName = "7A Utils/Balance/Formulas/Base"), Serializable]
public class BaseFormula : Formula
{
    public float value;

    public override float GetValue(float input)
    {
        return value;
    }
}