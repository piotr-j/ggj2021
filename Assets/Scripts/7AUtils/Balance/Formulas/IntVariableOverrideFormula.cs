using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "IntVariable Override Formula", menuName = "7A Utils/Balance/Formulas/Override - IntVariable"), Serializable]
public class IntVariableOverrideFormula : Formula
{
    public IntVariable var;
    
    public override float GetValue(float input)
    {
        return var.Value;
    }
}
