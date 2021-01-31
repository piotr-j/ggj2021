// ----------------------------------------------------------------------------
// Unite 2017 - Game Architecture with Scriptable Objects
// 
// Author: Ryan Hipple
// Date:   10/04/17
// ----------------------------------------------------------------------------

using UnityEngine;

[CreateAssetMenu(fileName = "Bool Variable", menuName = "7A Utils/Variables/Bool Variable")]
public class BoolVariable : ScriptableObject
{
#if UNITY_EDITOR
    [Multiline]
    public string DeveloperDescription = "";
#endif
    public bool Value { get; private set; }
    public bool DefaultValue = false;

    [SerializeField, ShowOnly] private bool m_CurrentValue;

    public BoolEvent onChange;

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

    public void SetValue(bool value)
    {
        Value = value;

        RiseOnChange();

        if (isPersistent) Save();
    }

    public void SetValue(BoolVariable value)
    {
        Value = value.Value;

        RiseOnChange();

        if (isPersistent) Save();
    }

    public void Save()
    {
        PlayerPrefs.SetInt(prefsName, Value ? 1 : 0);
        PlayerPrefs.Save();
    }

    public void Load()
    {
        Value = PlayerPrefs.GetInt(prefsName, DefaultValue ? 1 : 0) == 1 ? true : false;

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