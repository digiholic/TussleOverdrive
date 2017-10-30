using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// CONTROL SUBACTION
/// Finishes up a conditional block, dropping the conditional depth one level.
/// No arguments required
/// </summary>
public class SubactionEndIf : Subaction {

    public override void Execute(BattleObject obj, GameAction action)
    {
        action.cond_depth--;
        if (action.cond_depth < 0) action.cond_depth = 0;
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
