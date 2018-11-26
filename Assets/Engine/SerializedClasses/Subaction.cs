using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Subaction : ScriptableObject
{
    public string SubactionName;
    public SubVarDict arg_dict = new SubVarDict();

    /// <summary>
    /// Executes the subaction
    /// </summary>
    /// <param name="actor">The BattleObject the subaction is being executed by</param>
    /// <param name="action">The action that is calling the subaction</param>
    public virtual void Execute(BattleObject actor, GameAction action)
    {

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
    public virtual bool canExecuteInBuilder()
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
    public virtual SubactionType getSubactionType()
    {
        return SubactionType.OTHER;
    }
}


[System.Serializable]
public enum SubactionType
{
    CONTROL,
    BEHAVIOR,
    ANIMATION,
    HITBOX,
    OTHER
}