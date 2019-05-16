using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// BEHAVIOR SUBACTION
/// Enters a system-defined Transition State
/// 
/// Arguments:
///     name - required string - the name of the transition state to use
/// </summary>
public class SubactionDoTransition : Subaction {
    public override void Execute(BattleObject obj, GameAction action)
    {
        string name = (string) GetArgument("transitionState", obj, action);
        StateTransitions.LoadTransitionState(name, obj.GetAbstractFighter());
    }

    public override SubactionType getSubactionType()
    {
        return SubactionType.BEHAVIOR;
    }

    /*
    public override void generateDefaultArguments()
    {
        arg_list.Add(new SubactionVarData("name", "constant", "string", "", false));
        BuildDict();
    }
    */
}
