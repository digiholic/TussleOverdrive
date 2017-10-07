using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttributeDataRow : MonoBehaviour {

    public string variable_name;
    public string variable_type;
    public string variable_value;

    public UILabel name_label;
    public UILabel type_label;
    public UIInput value_label;

    void OnEnable()
    {
        if (LegacyEditor.FighterLoaded) RefreshFighter(LegacyEditor.editor.current_fighter);
    }

    void RefreshFighter(FighterInfo info)
    {
        VarData vardata = info.GetVarByName(variable_name);
        name_label.text = vardata.name;
        type_label.text = vardata.type.ToString();
        value_label.text = vardata.value;
    }

    void OnSubmit(string value)
    {
        Debug.Log(value);
    }
}
