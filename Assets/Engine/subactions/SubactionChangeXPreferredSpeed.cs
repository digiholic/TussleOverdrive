using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SubactionChangeXPreferredSpeed : Subaction
{
    public override void Execute(BattleObject actor, GameAction action)
    {
        base.Execute(actor, action);
        bool relative = (bool) GetArgument("relative", actor, action, false);
        float x = (float) GetArgument("x", actor, action);
        if (relative) actor.SendMessage("ChangeXPreferredBy",x);
        else actor.SendMessage("ChangeXPreferred", x);
    }

    public override SubactionType getSubactionType()
    {
        return SubactionType.BEHAVIOR;
    }
}
