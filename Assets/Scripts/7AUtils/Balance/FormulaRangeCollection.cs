using System;
using UnityEngine;

[CreateAssetMenu(fileName = "Formula Range", menuName = "7A Utils/Balance/Ranges/Formula"), Serializable]
public class FormulaRangeCollection : RangeCollection<Formula, FormulaRangeNum>
{
    [Header("Config")]
    public bool isAccumulative;
	public bool isHardCap = false;
	[ConditionalField("isAccumulative")] public float baseValue;


    [Header("Testing")]
    [Space(10)]
    [TextAreaAttribute(1, 10)]
    public string sample;

    [ShowOnly, SerializeField]
    private string lastValue;
    
    [Space(5)]
    [SerializeField] private string testInput = "0";
    [ShowOnly, SerializeField] private string testOutput;



    public float GetValueAccumulative(int level)
    {
        int start = (int)GetFirstMin();
        //int end = Math.Min(level, (int)GetLastMax());
        float previous = baseValue;

        for (int i = start; i <= level; i++)
        {
            Formula formula = GetValue(i);

            previous = formula.GetValue(previous);
        }

        return previous;
    }


    public override void OnValidate()
    {
        base.OnValidate();

        if (ranges.Count == 0)
        {
            sample = "Range empty";
            return;
        }

        int start = (int)GetFirstMin();
        int end = (int)GetLastMax();
        float previous = baseValue;

        if (isAccumulative)
        {
            UpdateAccumulativeSample(start, end, previous);
        }
        else
        {
            UpdateSample(start, end);
        }
    }

    private void UpdateSample(int start, int end)
    {
        sample = "";

        for (int i = start; i <= Mathf.Min(end, 300); i++)
        {
            Formula formula = GetValue(i);
            sample += formula.GetValue(i) + "; ";
        }

        lastValue = GetValue((int)GetLastMax()).GetValue((int)GetLastMax()).ToString();

        try
        {
            int val = Int32.Parse(testInput);
            testOutput = GetValue(val).GetValue(val).ToString();
        } catch (Exception) { };
    }
    
    private void UpdateAccumulativeSample(int start, int end, float previous)
    {
        sample = "";

        for (int i = start; i <= Mathf.Min(end, 300); i++)
        {
            Formula formula = GetValue(i);
            previous = formula.GetValue(previous);
            sample += previous + "; ";
        }

        lastValue = GetValueAccumulative((int)GetLastMax()).ToString();

        try
        {
            testOutput = GetValueAccumulative(Int32.Parse(testInput)).ToString();
        } catch (Exception) { };
    }
}

[Serializable]
public class FormulaRangeNum : RangeNum<Formula>
{
}
