using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// CONTROL SUBACTION
/// Initializes a variable to a given value.
/// 
/// Arguments:
///     variable - required - The variable being set. Set this argument up like you were getting a variable, but it'll be used to set it instead
///     value - required - The value to set the variable to. Can be a constant, or the value or another variable.
/// </summary>
public class SubactionInitVar : Subaction {

    public override void Execute(BattleObject actor, GameAction action)
    {
        base.Execute(actor, action);
        SubactionVarData variable = arg_dict["variable"];
        SubactionVarData data = arg_dict["value"];
        if (variable.source == SubactionSource.OWNER){
            //Only set the variable if there isn't already one present
            if (!actor.HasVar(variable.data)){
                variable.SetVariableInTarget(actor,action,data.GetData(actor,action));
            }
        } else if (variable.source == SubactionSource.ACTION){
            //Only set the variable if there isn't already one present
            if (!action.HasVar(variable.data)){
                variable.SetVariableInTarget(actor,action,data.GetData(actor,action));
            }
        }
    }

    public override SubactionType getSubactionType()
    {
        return SubactionType.CONTROL;
    }
}
