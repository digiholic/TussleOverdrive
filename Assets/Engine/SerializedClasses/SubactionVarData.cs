using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class SubactionVarData
{
    //[System.NonSerialized]
    public bool editable = true;

    public string name; //The name of the variable in the subaction, so you can easily set arguments without needing to remember order
    public SubactionSource source; //Constant, Owner, or Action. The source of the variable
    public SubactionVarType type; //String, Int, Float, or Bool
    public string data; //The string representation of the data or the name of the variable to use

    [TextArea]
    public string description;

    public SubactionVarData(string _name, SubactionSource _source, SubactionVarType _type, string _data, string desc, bool _editable = true)
    {
        name = _name;
        source = _source;
        type = _type;
        data = _data;
        editable = _editable;
        description = desc;
    }

    public SubactionVarData Copy()
    {
        return new SubactionVarData(name, source, type, data, description, editable);
    }

    public object GetData(BattleObject owner, GameAction action)
    {
        if (source == SubactionSource.CONSTANT)
        {
            if (type == SubactionVarType.STRING) return data;
            else if (type == SubactionVarType.INT) return int.Parse(data);
            else if (type == SubactionVarType.FLOAT) return float.Parse(data);
            else if (type == SubactionVarType.BOOL) return bool.Parse(data);
            else
            {
                Debug.LogError("SubactionVarData incorrect type: " + type);
                return null;
            }
        }
        else if (source == SubactionSource.OWNER)
        {
            if (type == SubactionVarType.STRING) return owner.GetStringVar(data);
            else if (type == SubactionVarType.INT) return owner.GetIntVar(data);
            else if (type == SubactionVarType.FLOAT) return owner.GetFloatVar(data);
            else if (type == SubactionVarType.BOOL) return owner.GetBoolVar(data);
            else
            {
                Debug.LogError("SubactionVarData incorrect type: " + type);
                return null;
            }
        }
        else if (source == SubactionSource.ACTION)
        {
            if (type == SubactionVarType.STRING) return action.GetStringVar(data);
            else if (type == SubactionVarType.INT) return action.GetIntVar(data);
            else if (type == SubactionVarType.FLOAT) return action.GetFloatVar(data);
            else if (type == SubactionVarType.BOOL) return action.GetBoolVar(data);
            else
            {
                Debug.LogError("SubactionVarData incorrect type: " + type);
                return null;
            }
        }
        else
        {
            Debug.LogError("SubactionVarData incorrect source: " + source);
            return null;
        }
    }

    /// <summary>
    /// Sets the fighter or action variable that this data corresponds to to a given value.
    /// For example, if this SubactionVarData is meant to get the TussleConstants.FighterVariableNames.FACING_DIRECTION variable from fighter, you could use
    /// this function on it and pass it a value to set TussleConstants.FighterVariableNames.FACING_DIRECTION to the given value.
    /// 
    /// If called on a "constant" SubactionVarData, this function does nothing.
    /// </summary>
    /// <param name="owner">The BattleObject this is operating on</param>
    /// <param name="action">The action calling this subaction</param>
    /// <param name="value">The data to set the variable</param>
    public void SetVariableInTarget(BattleObject owner, GameAction action, object value)
    {
        if (source == SubactionSource.CONSTANT)
        {
            Debug.LogWarning("SetVariable given a constant instead of a variable");
        }
        else if (source == SubactionSource.OWNER)
        {
            owner.SetVar(data, value);
        }
        else if (source == SubactionSource.ACTION)
        {
            action.SetVar(data, value);
        }
    }

    public bool IsNumeric()
    {
        return (type == SubactionVarType.INT || type == SubactionVarType.INT);
    }

    public string SourceAsString()
    {
        switch (source)
        {
            case (SubactionSource.CONSTANT):
                return "Constant";
            case (SubactionSource.OWNER):
                return "Owner";
            case (SubactionSource.ACTION):
                return "Action";
            default:
                return "";
        }
    }

    public string TypeAsString()
    {
        switch (type)
        {
            case (SubactionVarType.STRING):
                return "string";
            case (SubactionVarType.INT):
                return "int";
            case (SubactionVarType.FLOAT):
                return "float";
            case (SubactionVarType.BOOL):
                return "bool";
            default:
                return "";
        }
    }

    public void SetSourceString(string sourceString)
    {
        if (sourceString == "Constant")
            source = SubactionSource.CONSTANT;
        else if (sourceString == "Owner")
            source = SubactionSource.OWNER;
        else if (sourceString == "Action")
            source = SubactionSource.ACTION;
        else
            Debug.LogError("Invalid source passed to SubactionVarData: " + sourceString);
    }

    public void SetTypeString(string typeString)
    {
        if (typeString == "string")
            type = SubactionVarType.STRING;
        if (typeString == "int")
            type = SubactionVarType.INT;
        if (typeString == "float")
            type = SubactionVarType.FLOAT;
        if (typeString == "bool")
            type = SubactionVarType.BOOL;
        else
            Debug.LogError("Invalid type passed to SubactionVarData: " + typeString);
    }
}

public enum SubactionSource
{
    CONSTANT,
    OWNER,
    ACTION
}

public enum SubactionVarType
{
    STRING,
    BOOL,
    INT,
    FLOAT
}