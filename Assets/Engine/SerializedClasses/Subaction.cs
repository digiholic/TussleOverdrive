using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Subaction
{
    public string SubactionName;
    public List<SubactionVarData> arg_list;
    protected Dictionary<string, SubactionVarData> arg_dict;

    /// <summary>
    /// Builds the dictionary of variables keyed by name for easier access.
    /// Called when the subaction is generated.
    /// </summary>
    protected void BuildDict()
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
        //Since subaction types can't be seperate objects, the only thing we can do is a big honking case block.
        //Sorry.

        switch (SubactionName)
        {
            case "ModifyHitbox":
                /* createHitbox name:string [argumentName:string value:dynamic]
                 *      Creates a hitbox with the given name. Every pair of arguments from then after is the name of a value, and what to set it to.
                 *      Hitboxes will be able to parse the property name and extract the right value out.
                 */
                {
                    string name = "";
                    Dictionary<string, string> hbox_dict = new Dictionary<string, string>();
                    foreach (SubactionVarData data in arg_list)
                    {
                        if (data.name == "name")
                            name = (string)data.GetData(actor, action);
                        else
                        {
                            hbox_dict.Add(data.name, (string)data.GetData(actor, action));
                        }
                    }
                    if (name != "" && action.hitboxes.ContainsKey(name))
                    {
                        action.hitboxes[name].LoadValuesFromDict(actor.GetAbstractFighter(), hbox_dict);
                    }
                }
                break;
            default:
                //Debug.LogWarning("Could not load subaction " + args[0]);
                break;
        }
    }

    /// <summary>
    /// A quick helper function for execute to get the data from the arg dict
    /// </summary>
    /// <param name="arg_name"></param>
    /// <param name="owner"></param>
    /// <param name="action"></param>
    /// <returns></returns>
    public object GetArgument(string arg_name, BattleObject owner, GameAction action, object defaultValue=null)
    {
        if (arg_dict == null) BuildDict();
        if (arg_dict.ContainsKey(arg_name))
        {
            return arg_dict[arg_name].GetData(owner,action);
        } else
        {
            Debug.LogWarning("Argument not found in subaction: " + arg_name);
        }
        return defaultValue;
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
    public virtual bool isConditional()
    {
        return false;
    }

    /// <summary>
    /// Check if the subaction should execute in build mode, like animation and control flow subactions.
    /// </summary>
    /// <returns>If the subaction should be executed in the builder</returns>
    public virtual bool executeInBuilder()
    {
        return false;
    }

    /// <summary>
    /// Adds the subaction's default arguments to the subaction. Flags those subactions as non-renameable in builder.
    /// </summary>
    public virtual void generateDefaultArguments()
    {

    }


    /// <summary>
    /// Get the category this subaction belongs to.
    /// </summary>
    /// <returns></returns>
    public virtual SubactionCategory getCategory()
    {
        return SubactionCategory.OTHER;
    }
}


[System.Serializable]
public enum SubactionCategory
{
    CONTROL,
    BEHAVIOR,
    ANIMATION,
    HITBOX,
    OTHER
}

[System.Serializable]
public class SubactionVarData
{
    private bool editable = true;

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
            owner.SetVar(data,value);
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
