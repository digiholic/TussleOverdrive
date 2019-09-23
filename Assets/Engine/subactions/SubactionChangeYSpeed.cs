using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SubactionChangeYSpeed : Subaction
{
    public override void Execute(BattleObject actor, GameAction action)
    {
        base.Execute(actor, action);
        bool relative = (bool) GetArgument("relative", actor, action, false);
        float y = (float) GetArgument("y", actor, action);
        if (relative) actor.SendMessage("ChangeYSpeedBy",y);
        else actor.SendMessage("ChangeYSpeed", y);
    }

    public override SubactionType getSubactionType()
    {
        return SubactionType.BEHAVIOR;
    }
}
