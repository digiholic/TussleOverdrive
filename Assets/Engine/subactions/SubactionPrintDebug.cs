using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SubactionPrintDebug : Subaction
{
    public override void Execute(BattleObject actor, GameAction action)
    {
        Debug.Log(GetArgument("Text", actor, action));
    }

    public override SubactionType getSubactionType()
    {
        return SubactionType.CONTROL;
    }
}
