using System;
using System.Collections.Generic;
using UnityEngine;

public class RangeCollection<T, V> : ScriptableObject where V : RangeNum<T>
{
    public List<V> ranges;

    public T GetValue(float numberInRange)
    {
        return GetRange(numberInRange, true).value;
    }

    public T GetValue(float numberInRange, T defaultValueIfNotFound)
    {
        RangeNum<T> range = GetRange(numberInRange, false);

        if (range != null) return range.value;

        return defaultValueIfNotFound;
    }

    public RangeNum<T> GetRange(float numberInRange, bool lastIfNotFound)
    {

        for (int i = 0; i < ranges.Count; i++)
        {
            if (numberInRange >= ranges[i].min && numberInRange <= ranges[i].max)
            {
                return ranges[i];
            }
        }

        if (lastIfNotFound) return ranges[ranges.Count - 1];

        return null;
    }

    public RangeNum<T> GetNextRange(RangeNum<T> range)
    {
        for (int i = 0; i < ranges.Count; i++)
        {
            if (ranges[i] == range)
            {
                if (i + 1 >= ranges.Count)
                {
                    return null;
                }
                else
                {
                    return ranges[i + 1];
                }
            }
        }

        return null;
    }

    public float GetFirstMin()
    {
        if (ranges.Count == 0)
            return 0;

        return ranges[0].min;
    }

    public float GetLastMax()
    {
        if (ranges.Count == 0)
            return 0;

        return ranges[ranges.Count - 1].max;
    }

    public virtual void OnValidate()
    {
        foreach(V range in ranges)
        {
            range.UpdateName();
        }
    }


}

[Serializable]
public class RangeNum<V>
{
    [HideInInspector]
    public string name;

    public float min;
    public float max;
    public V value;

    public void UpdateName()
    {
        string range = min == max ? min.ToString() : min + " - " + max;

        range = range.PadRight(10);

        string output = value != null ? value.ToString() : "EMPTY";

        name = range + " > " + output;
    }
    
    //public RangeNum(float min, float max, V value)
    //{
    //    this.min = min;
    //    this.max = max;
    //    this.value = value;
    //}
}

