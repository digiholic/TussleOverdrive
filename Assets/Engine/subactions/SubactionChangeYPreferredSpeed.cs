using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SubactionChangeYPreferredSpeed : Subaction
{
    public override void Execute(BattleObject actor, GameAction action)
    {
        base.Execute(actor, action);
        bool relative = (bool) GetArgument("relative", actor, action, false);
        float y = (float) GetArgument("y", actor, action);
        if (relative) actor.SendMessage("ChangeYPreferredBy",y);
        else actor.SendMessage("ChangeYPreferred", y);
    }

    public override SubactionType getSubactionType()
    {
        return SubactionType.BEHAVIOR;
    }
}
