using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FighterDataSetter : MonoBehaviour {
    public string variable_name;
    public UILabel display_area;

	void RefreshFighter(FighterInfo info)
    {
        display_area.text = info.GetType().GetField(variable_name).GetValue(info).ToString();
    }

    void DataChanged(string text)
    {
        FighterInfo info = LegacyEditor.editor.current_fighter;
        info.GetType().GetField(variable_name).SetValue(info, text);
    }
}
