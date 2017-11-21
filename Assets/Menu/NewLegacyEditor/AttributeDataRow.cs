using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttributeDataRow : MonoBehaviour {
    public VarData vardata;

    public string variable_name;
    
    public UILabel name_label;
    public UIPopupList type_label;
    public UIInput value_label;

    public UIInputValidator value_validator;

    private UIGrid grid;

    void Start()
    {
        if (LegacyEditor.FighterLoaded) FighterChanged(LegacyEditor.editor.current_fighter);
        grid = transform.parent.GetComponent<UIGrid>();
    }

    void FighterChanged(FighterInfo info)
    {
        if (variable_name != "")
        {
            vardata = info.GetVarByName(variable_name);
            name_label.text = vardata.name;
            type_label.selection = vardata.type.ToString();
            value_label.text = vardata.value;
        }
    }

    void OnSubmit(string value)
    {
        Debug.Log(value);
    }

    void OnTypeChange(string value)
    {
        VarType vtype = VarType.STRING;
        if (value == "FLOAT")
        {
            vtype = VarType.FLOAT;
            value_validator.logic = UIInputValidator.Validation.Float;
        }
        if (value == "INT")
        {
            vtype = VarType.INT;
            value_validator.logic = UIInputValidator.Validation.Integer;
        }
        if (value == "BOOL")
        {
            vtype = VarType.BOOL;
            value_validator.logic = UIInputValidator.Validation.Alphanumeric;
        }
        if (value == "STRING")
        {
            vtype = VarType.STRING;
            value_validator.logic = UIInputValidator.Validation.Alphanumeric;
        }

        if (vtype != vardata.type) {
            vardata.type = vtype;
            vardata.value = "";
            value_label.text = "";
        }
    }

    void DeleteAttribute()
    {
        //Remove it from the fighter
        LegacyEditor.editor.current_fighter.variables.Remove(vardata);
        NGUITools.Destroy(gameObject);
    }

    void OnDestroy()
    {
        grid.Reposition();
    }

    public void SetNext(AttributeDataRow data)
    {
        value_label.selectOnTab = data.value_label.gameObject;
    }
}
