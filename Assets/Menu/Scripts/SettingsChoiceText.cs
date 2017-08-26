using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SettingsChoiceText : MonoBehaviour {
    public string var_name;
    public string display_text;
    public int value;
    public string after_text;

    public List<string> text_values;
    
    private UILabel label;
    

    void Start()
    {
        label = GetComponentInChildren<UILabel>();
        RedrawText();
    }

    void RedrawText()
    {
        label.text = display_text + text_values[value] + after_text;
    }

    void IncrementValue()
    {
        if (value == text_values.Count - 1)
            value = 0;
        else value++;
        RedrawText();
    }

    void DecrementValue()
    {
        if (value == 0)
            value = text_values.Count - 1;
        else value--;
        RedrawText();
    }
}
