using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// CONTROL SUBACTION
/// Sets a variable to a given value.
/// 
/// Arguments:
///     variable - required - The variable being set. Set this argument up like you were getting a variable, but it'll be used to set it instead
///     value - required - The value to set the variable to. Can be a constant, or the value or another variable.
/// </summary>
public class SubactionSetVar : Subaction {

    public override void Execute(BattleObject actor, GameAction action)
    {
        base.Execute(actor, action);
        arg_dict["variable"].SetVariable(actor, action, arg_dict["value"].GetData(actor, action));
    }

    public override SubactionType getSubactionType()
    {
        return SubactionType.CONTROL;
    }
    
    /*
    public override void generateDefaultArguments()
    {
        arg_list.Add(new SubactionVarData("variable", "constant", "string", "", false));
        arg_list.Add(new SubactionVarData("value", "constant", "string", "", false));
        BuildDict();
    }
    */
}
