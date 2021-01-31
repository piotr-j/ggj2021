// ----------------------------------------------------------------------------
// Unite 2017 - Game Architecture with Scriptable Objects
// 
// Author: Ryan Hipple
// Date:   10/04/17
// ----------------------------------------------------------------------------

using UnityEngine;

[CreateAssetMenu(fileName = "String Variable", menuName = "7A Utils/Variables/String Variable")]
public class StringVariable : ScriptableObject
{
#if UNITY_EDITOR
    [Multiline]
    public string DeveloperDescription = "";
#endif
    public string Value { get; private set; }
    public string DefaultValue = "";

    [SerializeField, ShowOnly] private string m_CurrentValue;

    public StringEvent onChange;

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

    public void SetValue(string value)
    {
        Value = value;

        RiseOnChange();

        if (isPersistent) Save();
    }

    public void SetValue(StringVariable value)
    {
        Value = value.Value;

        RiseOnChange();

        if (isPersistent) Save();
    }

    public void Save()
    {
        PlayerPrefs.SetString(prefsName, Value);
        PlayerPrefs.Save();
    }

    public void Load()
    {
        Value = PlayerPrefs.GetString(prefsName, DefaultValue);
        m_CurrentValue = Value;
    }

    public void Clear()
    {
        PlayerPrefs.DeleteKey(prefsName);
        PlayerPrefs.Save();
    }

    public void RiseOnChange()
    {
        onChange?.Raise(Value);
        m_CurrentValue = Value;
    }
    
}