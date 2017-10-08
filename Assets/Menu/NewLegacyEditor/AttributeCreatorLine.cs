using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttributeCreatorLine : MonoBehaviour {

    public UIInput variable_name;
    
    void NewAttribute()
    {
        if (variable_name.text != "")
        {
            VarData item = new VarData();
            item.name = variable_name.text;
            item.type = VarType.FLOAT;
            item.value = "0";
            LegacyEditor.editor.current_fighter.variables.Add(item);
            LegacyEditor.RefreshFighter();
            variable_name.text = "";
        }
    }
}