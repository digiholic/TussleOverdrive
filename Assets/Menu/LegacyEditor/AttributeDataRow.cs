using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttributeDataRow : MonoBehaviour, LegacyDataViewer {
    public VarData vardata;

    public string variable_name;
    
    public UILabel name_label;
    public UIPopupList type_label;
    public UIInput value_label;
    public UIInputValidator value_validator;

    public bool removable = false;
    public UIButton deleteButton;

    private UIGrid grid;

    void Start()
    {
        if (LegacyEditor.FighterLoaded) FighterChanged(LegacyEditor.editor.current_fighter);
        grid = transform.parent.GetComponent<UIGrid>();
    }

    public void SetRemovable(bool rem)
    {
        removable = rem;
        NGUITools.SetActive(deleteButton.gameObject, rem);
    }

    public void FighterChanged(FighterInfo info)
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
        vardata.value = value;
        LegacyEditor.editor.current_fighter.CreateOrUpdateVarData(vardata);
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


    public void WindowChanged(string window_name) { }
    public void SubWindowChanged(string sub_window_name) { }
    public void ActionFileChanged(ActionFile actions) { }
    public void SelectedActionChanged(DynamicAction action) { }
    public void CategoryChanged(string category_name) { }
    
    public void OnEnable()
    {
        LegacyEditor.OnFighterChanged += FighterChanged;
    }

    public void OnDisable()
    {
        LegacyEditor.OnFighterChanged -= FighterChanged;
    }
}
