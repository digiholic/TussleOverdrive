using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Subaction
{
    public string SubactionName;
    public List<SubactionVarData> arg_list;
    private Dictionary<string, SubactionVarData> arg_dict;

    private void BuildDict()
    {
        arg_dict = new Dictionary<string, SubactionVarData>();
        foreach (SubactionVarData data in arg_list)
        {
            arg_dict[data.name] = data;
        }
    }

    public virtual void Execute(BattleObject actor, GameAction action)
    {
        
    }

    public virtual List<string> GetRequirements()
    {
        return null;
    }
}


[System.Serializable]
public class SubactionVarData
{
    public string name; //The name of the variable in the subaction, so you can easily set arguments without needing to remember order
    public string source; //Constant, Owner, or Action. The source of the variable
    public string type; //String, Int, Float, or Bool
    public string data; //The string representation of the data or the name of the variable to use

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
}
