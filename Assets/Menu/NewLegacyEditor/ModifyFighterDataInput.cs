using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ModifyFighterDataInput : MonoBehaviour {
    public enum FighterVarType
    {
        FIELD,
        VARIABLE
    }

    public FighterVarType varType;
    public string varName;
    private UIInput input;
    private InputBoxFilter filter;

    private void Start()
    {
        input = GetComponent<UIInput>();
        filter = GetComponent<InputBoxFilter>();
    }

    private void OnSelect()
    {
        //input.text = getFighterVar();
    }

    private void OnModelChanged()
    {
        if (LegacyEditorData.instance.loadedFighterDirty)
        {
            input.text = getFighterVar();
        }
    }

    public void OnAction(string inputData)
    {
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

    private string getFighterVar()
    {
        FighterInfo info = LegacyEditorData.instance.loadedFighter;
        if (varType == FighterVarType.FIELD)
        {
            return (string)info.GetType().GetField(varName).GetValue(info);
        }
        else
        {
            return info.GetVarByName(varName).value;
        }
    }

}
