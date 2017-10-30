using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// BEHAVIOR SUBACTION
/// Sets the object's speed, or preferred speed. If a variable is not set,
/// it will not be changed. If relative is set, the speed will be changed
/// relatively.
/// 
/// Arguments:
///     xspeed - optional float - The X speed to set
///     yspeed - optional float - The Y speed to set
///     xpref  - optional float - The X preferred speed to set
///     ypref  - optional float - The X preferred speed to set
///     relative - optional bool (default true) - If true, speed will be incremented rather than set
/// </summary>
public class SubactionChangeSpeed : Subaction {

    public override void Execute(BattleObject actor, GameAction action)
    {
        base.Execute(actor, action);
        bool relative = (bool) GetArgument("relative", actor, action, false);
        if (arg_dict.ContainsKey("xspeed"))
        {
            if (relative) actor.SendMessage("ChangeXSpeedBy", GetArgument("xspeed", actor, action));
            else actor.SendMessage("ChangeXSpeed", GetArgument("xspeed", actor, action));
        }
        if (arg_dict.ContainsKey("yspeed"))
        {
            if (relative) actor.SendMessage("ChangeYSpeedBy", GetArgument("yspeed", actor, action));
            else actor.SendMessage("ChangeYSpeed", GetArgument("yspeed", actor, action));
        }
        if (arg_dict.ContainsKey("xpref"))
        {
            if (relative) actor.SendMessage("ChangeXPreferredBy", GetArgument("xpref", actor, action));
            else actor.SendMessage("ChangeXPreferred", GetArgument("xpref", actor, action));
        }
        if (arg_dict.ContainsKey("ypref"))
        {
            if (relative) actor.SendMessage("ChangeYPreferredBy", GetArgument("ypref", actor, action));
            else actor.SendMessage("ChangeYPreferred", GetArgument("ypref", actor, action));
        }
    }

    public override SubactionCategory getCategory()
    {
        return SubactionCategory.BEHAVIOR;
    }
}
