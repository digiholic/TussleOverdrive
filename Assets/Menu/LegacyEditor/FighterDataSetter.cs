using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FighterDataSetter : MonoBehaviour, LegacyDataViewer {
    public string variable_name;
    public UILabel display_area;

    public void FighterChanged(FighterInfo info)
    {
        display_area.text = info.GetType().GetField(variable_name).GetValue(info).ToString();
    }

    void DataChanged(string text)
    {
        FighterInfo info = LegacyEditor.editor.current_fighter;
        info.GetType().GetField(variable_name).SetValue(info, text);
    }

    public void ActionsChanged(ActionFile actions) { }
    public void CategoryChanged(string category_name) { }
    public void SelectedActionChanged(DynamicAction action) { }
    public void SubactionGroupChanged(SubActionGroup group) { }
    public void SubWindowChanged(string sub_window_name) { }
    public void WindowChanged(string window_name) { }
}
