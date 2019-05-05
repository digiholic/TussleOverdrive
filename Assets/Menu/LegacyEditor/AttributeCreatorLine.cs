using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttributeCreatorLine : MonoBehaviour {

    public UIInput variable_name;
    
    void NewAttribute()
    {
        if (variable_name.value != "")
        {
            VarData item = new VarData(variable_name.value,"0",VarType.FLOAT);
            LegacyEditor.editor.current_fighter.variables.Add(item);
            LegacyEditor.FireChangeFighter(LegacyEditor.editor.current_fighter);
            variable_name.value = "";
        }
    }
}