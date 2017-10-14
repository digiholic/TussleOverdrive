using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Subaction
{
    
    public string SubactionName;
    public List<SubactionVarData> arg_list;
    private Dictionary<string, SubactionVarData> arg_dict;

    //This is used to denote if a subaction should be executed, regardless of conditional
    private bool is_control_subaction = false;
    //This is used to denote if a subaction should execute in the builder
    private bool execute_in_builder = false;

    /// <summary>
    /// Builds the dictionary of variables keyed by name for easier access.
    /// Called when the subaction is generated.
    /// </summary>
    private void BuildDict()
    {
        arg_dict = new Dictionary<string, SubactionVarData>();
        foreach (SubactionVarData data in arg_list)
        {
            arg_dict[data.name] = data;
        }
    }

    /// <summary>
    /// Executes the subaction
    /// </summary>
    /// <param name="actor">The BattleObject the subaction is being executed by</param>
    /// <param name="action">The action that is calling the subaction</param>
    public virtual void Execute(BattleObject actor, GameAction action)
    {
        
    }

    /// <summary>
    /// Get a list of modules that are required for the subaction to execute
    /// </summary>
    /// <returns>A list of names of modules that are required by the subaction</returns>
    public virtual List<string> GetRequirements()
    {
        return null;
    }

    /// <summary>
    /// Check if the subaction is a conditional subaction, which would execute even if the conditional flag isn't set for execution.
    /// </summary>
    /// <returns>If the subaction should be executed regardless of conditional status</returns>
    public bool isConditional()
    {
        return is_control_subaction;
    }

    /// <summary>
    /// Check if the subaction should execute in build mode, like animation and control flow subactions.
    /// </summary>
    /// <returns>If the subaction should be executed in the builder</returns>
    public bool executeInBuilder()
    {
        return execute_in_builder;
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
