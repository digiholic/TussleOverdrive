using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ModifyDataInputBox : LegacyEditorWidget {
    public enum DataInputSource
    {
        FIGHTER,
        ACTION
    }
    public enum DataInputVarSource
    {
        FIELD,
        PROPERTY,
        VARIABLE
    }
    public DataInputSource source;
    public DataInputVarSource varSource;
    public VarType varType;
    public string varName;
    private UIInput input;
    private InputBoxFilter filter;

    private void Awake()
    {
        input = GetComponent<UIInput>();
        filter = GetComponent<InputBoxFilter>();
    }

    void OnFighterInfoChanged(FighterInfo info)
    {
        if (source == DataInputSource.FIGHTER)
        {
            input.value = getFighterVar(info);
        }
    }

    void OnActionChanged(DynamicAction action)
    {
        if (source == DataInputSource.ACTION)
        {
            input.value = getActionVar().ToString();
        }
    }

    public void OnAction()
    {
        string inputData = input.value;

        //If we have a filter object, make sure to filter the incoming text before we do anything with it.
        if (filter != null) inputData = filter.filterText(inputData);

        LegacyEditorAction action = null;
        if (varSource == DataInputVarSource.FIELD)
        {
            if (source == DataInputSource.FIGHTER)
            {
                action = ScriptableObject.CreateInstance<ChangeFighterInfoField>();
                ((ChangeFighterInfoField)action).init(varName, inputData);
            } else if (source == DataInputSource.ACTION)
            {
                action = ScriptableObject.CreateInstance<ChangeActionField>();
                ((ChangeActionField)action).init(varName, stringToObjectType(inputData));
            }
        }
        else if (varSource == DataInputVarSource.VARIABLE)
        {
            if (source == DataInputSource.FIGHTER)
            {
                action = ScriptableObject.CreateInstance<ChangeFighterInfoVar>();
                ((ChangeFighterInfoVar)action).init(varName, inputData);
            }
            else if (source == DataInputSource.ACTION)
            {
                action = ScriptableObject.CreateInstance<ChangeActionField>();
                ((ChangeActionField)action).init(varName, stringToObjectType(inputData));
            }
        }
        LegacyEditorData.instance.DoAction(action);
    }

    private string getFighterVar(FighterInfo info)
    {
        if (varSource == DataInputVarSource.FIELD)
        {
            return (string)info.GetType().GetField(varName).GetValue(info);
        } else if (varSource == DataInputVarSource.PROPERTY){
            return (string)info.GetType().GetProperty(varName).GetValue(info);
        }
        else
        {
            return info.GetVarByName(varName).value;
        }
    }

    private object getActionVar()
    {
        DynamicAction action = LegacyEditorData.instance.currentAction;
        if (varSource == DataInputVarSource.FIELD)
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

    public override void RegisterListeners()
    {
        editor.FighterInfoChangedEvent += OnFighterInfoChanged;
    }

    public override void UnregisterListeners()
    {
        editor.FighterInfoChangedEvent -= OnFighterInfoChanged;
    }
}
