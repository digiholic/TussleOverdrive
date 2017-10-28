using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SubactionElse : Subaction {

    public SubactionElse()
    {
    }

    public override void Execute(BattleObject obj, GameAction action)
    {
        action.cond_list[action.cond_depth] = !action.cond_list[action.cond_depth];
    }

    public override List<string> GetRequirements()
    {
        List<string> retList = new List<string>();
        retList.Add("ActionHandler");
        return retList;
    }
}
