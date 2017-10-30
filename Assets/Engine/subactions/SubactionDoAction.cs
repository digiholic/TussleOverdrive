using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SubactionDoAction : Subaction {

    public override void Execute(BattleObject obj, GameAction action)
    {
        obj.BroadcastMessage("DoAction", arg_dict["actionName"].GetData(obj,action));
    }

    public override SubactionCategory getCategory()
    {
        return SubactionCategory.CONTROL;
    }
}