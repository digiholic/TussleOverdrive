using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ModifyActionDataInput : MonoBehaviour
{
    public enum ActionVarType
    {
        FIELD,
        VARIABLE
    }

    public VarType varType;
    public ActionVarType varSource;
    public string varName;
    private UIInput input;
    private InputBoxFilter filter;

    private void Awake()
    {
        input = GetComponent<UIInput>();
        filter = GetComponent<InputBoxFilter>();
    }

    private void OnModelChanged()
    {
        if (LegacyEditorData.instance.currentActionDirty)
        {
            input.value = getActionVar().ToString();
        }
    }

    public void OnAction(string inputData)
    {
        //If we have a filter object, make sure to filter the incoming text before we do anything with it.
        if (filter != null) inputData = filter.filterText(inputData);

        LegacyEditorAction action = null;
        if (varSource == ActionVarType.FIELD)
        {
            action = ScriptableObject.CreateInstance<ChangeActionField>();
            ((ChangeActionField)action).init(varName, stringToObjectType(inputData));
        }
        else if (varSource == ActionVarType.VARIABLE)
        {
            action = ScriptableObject.CreateInstance<ChangeActionField>();
            ((ChangeActionField)action).init(varName, stringToObjectType(inputData));
        }
        LegacyEditorData.instance.DoAction(action);
    }

    private object getActionVar()
    {
        DynamicAction action = LegacyEditorData.instance.currentAction;
        if (varSource == ActionVarType.FIELD)
        {
            object actionVar = action.GetType().GetField(varName).GetValue(action);
            if (actionVar != null)
            {
                switch (varType)
                {
                    case VarType.BOOL:
                        return (bool)actionVar;
                    case VarType.INT:
                        return (int)actionVar;
                    case VarType.FLOAT:
                        return (float)actionVar;
                    default:
                        return (string)actionVar;
                }
            }
            else
            {
                Debug.LogWarning("Action " + action.name + " has null value for var: " + varName);
                return "";
            }
        }
        //else
        //{
        //return action.GetVar(varName);
        //}
        return "";
    }

    private object stringToObjectType(string val)
    {
        switch (varType)
        {
            case VarType.BOOL:
                return bool.Parse(val);
            case VarType.INT:
                return int.Parse(val);
            case VarType.FLOAT:
                return float.Parse(val);
            default:
                return val;
        }
    }

}

