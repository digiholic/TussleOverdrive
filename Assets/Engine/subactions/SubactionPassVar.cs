using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// CONTROL SUBACTION
/// Pass a variable to the next action after this one. CAUTION: Call this right before switching to the intended action to avoid unexpected consequences. Behaves as if you had called SetVar on the next action.
/// </summary>
public class SubactionPassVar : Subaction {

    public override void Execute(BattleObject actor, GameAction action)
    {
        base.Execute(actor, action);
        action.PassVariable(GetArgument("varName",actor,action).ToString(),GetArgument("value",actor,action));
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
