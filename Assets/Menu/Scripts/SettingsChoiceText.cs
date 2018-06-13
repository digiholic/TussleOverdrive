using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SettingsChoiceText : MonoBehaviour {
    public string var_name;
    public string display_text;
    public int value;
    public string after_text;
    public bool isBool;
    public List<string> text_values;

    private UILabel label;
    [SerializeField]
    private UILabel choiceLabel;

    void Start()
    {
        label = transform.Find("VarName").GetComponent<UILabel>();
        choiceLabel = transform.Find("ChoiceText").GetComponent<UILabel>();
        init();
    }

    public void init()
    {
        if (isBool)
            value = ((bool)Settings.current_settings.GetSetting(var_name) ? 1 : 0);
        else
            value = text_values.IndexOf(Settings.current_settings.GetSetting(var_name).ToString());

        RedrawText();
    }

    void RedrawText()
    {
        label.text = display_text;
        choiceLabel.text = text_values[value] + after_text;
        Settings.current_settings.ChangeSetting(var_name, text_values[value]);
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
