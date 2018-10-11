using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class SubactionVarData
{
    //[System.NonSerialized]
    public bool editable = true;

    public string name; //The name of the variable in the subaction, so you can easily set arguments without needing to remember order
    public string source; //Constant, Owner, or Action. The source of the variable
    public string type; //String, Int, Float, or Bool
    public string data; //The string representation of the data or the name of the variable to use

    public SubactionVarData(string _name, string _source, string _type, string _data, bool _editable = true)
    {
        name = _name;
        source = _source;
        type = _type;
        data = _data;
        editable = _editable;
    }

    public object GetData(BattleObject owner, GameAction action)
    {
        if (source == "Constant")
        {
            if (type == "string") return data;
            else if (type == "int") return int.Parse(data);
            else if (type == "float") return float.Parse(data);
            else if (type == "bool") return bool.Parse(data);
            else
            {
                Debug.LogError("SubactionVarData incorrect type: " + type);
                return null;
            }
        }
        else if (source == "Owner")
        {
            if (type == "string") return owner.GetStringVar(data);
            else if (type == "int") return owner.GetIntVar(data);
            else if (type == "float") return owner.GetFloatVar(data);
            else if (type == "bool") return owner.GetBoolVar(data);
            else
            {
                Debug.LogError("SubactionVarData incorrect type: " + type);
                return null;
            }
        }
        else if (source == "Action")
        {
            if (type == "string") return action.GetStringVar(data);
            else if (type == "int") return action.GetIntVar(data);
            else if (type == "float") return action.GetFloatVar(data);
            else if (type == "bool") return action.GetBoolVar(data);
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
    /// For example, if this SubactionVarData is meant to get the "facing" variable from fighter, you could use
    /// this function on it and pass it a value to set "facing" to the given value.
    /// 
    /// If called on a "constant" SubactionVarData, this function does nothing.
    /// </summary>
    /// <param name="owner">The BattleObject this is operating on</param>
    /// <param name="action">The action calling this subaction</param>
    /// <param name="value">The data to set the variable</param>
    public void SetVariable(BattleObject owner, GameAction action, object value)
    {
        if (source == "Constant")
        {
            Debug.LogWarning("SetVariable given a constant instead of a variable");
        }
        else if (source == "Owner")
        {
            owner.SetVar(data, value);
        }
        else if (source == "Action")
        {
            action.SetVar(data, value);
        }
    }

    public bool IsNumeric()
    {
        return (type == "int" || type == "float");
    }
}