using System;
using UnityEngine;

[CreateAssetMenu(fileName = "Mul Formula", menuName = "7A Utils/Balance/Formulas/Multiply"), Serializable]
public class MulFormula : Formula
{
    public float value;

    public override float GetValue(float input)
    {
        return input * value;
    }
}