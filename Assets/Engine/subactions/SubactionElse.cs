using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// CONTROL SUBACTION
/// Inverts the current conditional bit.
/// No arguments required.
/// </summary>
public class SubactionElse : Subaction {

    public override void Execute(BattleObject obj, GameAction action)
    {
        action.cond_list[action.cond_depth] = !action.cond_list[action.cond_depth];
    }

    public override SubactionCategory getCategory()
    {
        return SubactionCategory.CONTROL;
    }

    public override bool isConditional()
    {
        return true;
    }
}
