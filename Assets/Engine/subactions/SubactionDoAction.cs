using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SubactionDoAction : Subaction {

    public override void Execute(BattleObject obj, GameAction action)
    {
        obj.BroadcastMessage("DoAction", arg_dict["actionName"].GetData(obj,action));
    }

    public override SubactionType getSubactionType()
    {
        return SubactionType.CONTROL;
    }

    /*
    public override void generateDefaultArguments()
    {
        arg_list.Add(new SubactionVarData("actionName", "constant", "string", "", false));
        BuildDict();
    }
    */
}