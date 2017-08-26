using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SettingsNumberText : MonoBehaviour {
    public string var_name;
    public string display_text;
    public int value;
    public string after_text;

    public int min_value;
    public int max_value;
    public int increment;

    private UILabel label;

    void Start()
    {
        label = GetComponentInChildren<UILabel>();
        RedrawText();
    }

    void RedrawText()
    {
        label.text = display_text + value.ToString() + after_text;
        Settings.current_settings.ChangeSetting(var_name, value);
    }

    void IncrementValue()
    {
        if (value < max_value)
            value += increment;
        else
            value = min_value;
        RedrawText();
    }

    void DecrementValue()
    {
        if (value > min_value)
            value -= increment;
        else
            value = max_value;
        RedrawText();
    }
}
