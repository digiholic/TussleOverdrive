using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SubactionChangeXSpeed : Subaction
{
    public override void Execute(BattleObject actor, GameAction action)
    {
        base.Execute(actor, action);
        bool relative = (bool) GetArgument("relative", actor, action, false);
        float x = (float) GetArgument("x", actor, action);
        if (relative) actor.SendMessage("ChangeXSpeedBy",x);
        else actor.SendMessage("ChangeXSpeed", x);
    }

    public override SubactionType getSubactionType()
    {
        return SubactionType.BEHAVIOR;
    }
}
