using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActionDataSetter : MonoBehaviour {
    public string variable_name;
    public UILabel display_area;
    public UICheckbox bool_display;

    void ActionChanged(DynamicAction action)
    {
        if (display_area != null)
            display_area.text = action.GetType().GetField(variable_name).GetValue(action).ToString();
        if (bool_display != null)
            bool_display.isChecked = (bool)action.GetType().GetField(variable_name).GetValue(action);
    }

    void DataChanged(string text)
    {
        if (display_area != null)
        {
            DynamicAction action = LegacyEditor.editor.selected_action;
            action.GetType().GetField(variable_name).SetValue(action, text);
        }
    }

    void DataChecked(bool check)
    {
        if (LegacyEditor.editor.selected_action != null && bool_display != null)
        {
            DynamicAction action = LegacyEditor.editor.selected_action;
            action.GetType().GetField(variable_name).SetValue(action, check);
        }
    }
}
