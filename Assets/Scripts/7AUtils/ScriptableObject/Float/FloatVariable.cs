// ----------------------------------------------------------------------------
// Unite 2017 - Game Architecture with Scriptable Objects
// 
// Author: Ryan Hipple
// Date:   10/04/17
// ----------------------------------------------------------------------------

using UnityEngine;

[CreateAssetMenu(fileName = "Float Variable", menuName = "7A Utils/Variables/Float Variable")]
public class FloatVariable : ScriptableObject
{
#if UNITY_EDITOR
    [Multiline]
    public string DeveloperDescription = "";
#endif
    public float Value { get; private set; }
    public int DefaultValue = 0;

    [SerializeField, ShowOnly] private float m_CurrentValue;

    public FloatEvent onChange;

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

    public void SetValue(float value)
    {
        Value = value;

        RiseOnChange();

        if (isPersistent) Save();
    }

    public void SetValue(FloatVariable value)
    {
        Value = value.Value;

        RiseOnChange();

        if (isPersistent) Save();
    }

    public void ApplyChange(float amount)
    {
        Value += amount;

        RiseOnChange();

        if (isPersistent) Save();
    }

    public void ApplyChange(FloatVariable amount)
    {
        Value += amount.Value;

        RiseOnChange();

        if (isPersistent) Save();
    }

    public void Save()
    {
        PlayerPrefs.SetFloat(prefsName, Value);
        PlayerPrefs.Save();
    }

    public void Load()
    {
        Value = PlayerPrefs.GetFloat(prefsName, DefaultValue);
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