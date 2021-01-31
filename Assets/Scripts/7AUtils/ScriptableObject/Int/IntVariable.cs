// ----------------------------------------------------------------------------
// Unite 2017 - Game Architecture with Scriptable Objects
// 
// Author: Ryan Hipple
// Date:   10/04/17
// ----------------------------------------------------------------------------

using UnityEngine;

[CreateAssetMenu(fileName = "Int Variable", menuName = "7A Utils/Variables/Int Variable")]
public class IntVariable : ScriptableObject
{
#if UNITY_EDITOR
    [Multiline]
    public string DeveloperDescription = "";
#endif
    public int Value { get; private set; }
    public int DefaultValue = 0;

    [SerializeField, ShowOnly] private int m_CurrentValue;

    public IntEvent onChange;

    [Header("Persistance")]
    public bool isPersistent;

    [ConditionalField("isPersistent")] public string prefsName;

    private void OnEnable()
    {
        if (isPersistent)
        {
            Load();
        }
        else
        {
            Value = DefaultValue;

            m_CurrentValue = Value;
        }

    }

    public void SetDefaultValue()
    {
        SetValue(DefaultValue);
    }
    
    public void SetValue(int value)
    {
        Value = value;

        RiseOnChange();

        if (isPersistent) Save();
    }

    public void SetValue(IntVariable value)
    {
        Value = value.Value;

        RiseOnChange();

        if (isPersistent) Save();
    }

    public void ApplyChange(int amount)
    {
        Value += amount;

        RiseOnChange();

        if (isPersistent) Save();
    }

    public void ApplyChange(IntVariable amount)
    {
        Value += amount.Value;

        RiseOnChange();

        if (isPersistent) Save();
    }

    public void Save()
    {
        PlayerPrefs.SetInt(prefsName, Value);
        PlayerPrefs.Save();
    }

    public void Load()
    {
        Value = PlayerPrefs.GetInt(prefsName, DefaultValue);
        m_CurrentValue = Value;
    }

    public void Clear()
    {
        PlayerPrefs.DeleteKey(prefsName);
        PlayerPrefs.Save();
    }

    private void RiseOnChange()
    {
        onChange?.Raise(Value);
        m_CurrentValue = Value;
    }
    
}