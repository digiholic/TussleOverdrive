using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ModifyFighterDataInput : LegacyEditorWidget {
    public enum FighterVarType
    {
        FIELD,
        VARIABLE
    }

    public FighterVarType varType;
    public string varName;
    private UIInput input;
    private InputBoxFilter filter;

    private void Awake()
    {
        input = GetComponent<UIInput>();
        filter = GetComponent<InputBoxFilter>();
    }

    private void OnSelect()
    {
        //input.vale = getFighterVar();
    }

    void OnFighterInfoChanged(FighterInfo info)
    {
        input.value = getFighterVar(info);
    }

    public void OnAction()
    {
        string inputData = input.value;

        //If we have a filter object, make sure to filter the incoming text before we do anything with it.
        if (filter != null) inputData = filter.filterText(inputData);

        LegacyEditorAction action = null;
        if (varType == FighterVarType.FIELD)
        {
            action = ScriptableObject.CreateInstance<ChangeFighterInfoField>();
            ((ChangeFighterInfoField)action).init(varName, inputData);
        }
        else if (varType == FighterVarType.VARIABLE)
        {
            action = ScriptableObject.CreateInstance<ChangeFighterInfoVar>();
            ((ChangeFighterInfoVar)action).init(varName, inputData);
        }
        LegacyEditorData.instance.DoAction(action);
    }

    private string getFighterVar(FighterInfo info)
    {
        if (varType == FighterVarType.FIELD)
        {
            return (string)info.GetType().GetField(varName).GetValue(info);
        }
        else
        {
            return info.GetVarByName(varName).value;
        }
    }

    public override void RegisterListeners()
    {
        editor.FighterInfoChangedEvent += OnFighterInfoChanged;
    }

    public override void UnregisterListeners()
    {
        editor.FighterInfoChangedEvent -= OnFighterInfoChanged;
    }
}
