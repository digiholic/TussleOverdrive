using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// CONTROL SUBACTION
/// Changes the next execution frame of the action. Note that the current frame finishes execution, and will switch to the given one for the next execution. Can be given a number, or if relative is true, will add the given number to the current frame instead.
/// 
/// Arguments:
///     frameNumber - required int - The Frame number to change to, or the amount to modify the current frame by.
///     relative - optional bool - If true, will modify the current frame instead of setting directly.
/// </summary>
public class SubactionChangeFrame : Subaction
{
    public override void Execute(BattleObject actor, GameAction action)
    {
        int frameNumber = (int)GetArgument("frameNumber", actor, action, 1);
        bool relative = (bool)GetArgument("relative", actor, action, false);
        action.ChangeFrame(frameNumber, relative);
    }

    public override SubactionType getSubactionType()
    {
        return SubactionType.CONTROL;
    }

    public override bool canExecuteInBuilder()
    {
        return true;
    }
}
