using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SubactionEndIf : Subaction {

    public SubactionEndIf()
    {
    }

    public override void Execute(BattleObject obj, GameAction action)
    {
        action.cond_depth--;
        if (action.cond_depth < 0) action.cond_depth = 0;
    }

    public override List<string> GetRequirements()
    {
        List<string> retList = new List<string>();
        retList.Add("ActionHandler");
        return retList;
    }
}
