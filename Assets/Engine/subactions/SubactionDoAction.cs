using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SubactionDoAction : Subaction {
    public string actionName;

    public SubactionDoAction(string _actionName)
    {
        actionName = _actionName;
    }

    public override void Execute(BattleObject obj, GameAction action)
    {
        obj.BroadcastMessage("DoAction", actionName);
    }

    public override List<string> GetRequirements()
    {
        List<string> retList = new List<string>();
        retList.Add("ActionHandler");
        return retList;
    }
}