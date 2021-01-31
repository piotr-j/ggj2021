using System;
using UnityEngine;

[CreateAssetMenu(fileName = "Add Formula", menuName = "7A Utils/Balance/Formulas/Add"), Serializable]
public class AddFormula : Formula
{
    public float value;

    public override float GetValue(float input)
    {
        return input + value;
    }
}
