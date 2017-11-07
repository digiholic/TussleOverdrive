using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// CONTROL SUBACTION
/// Sets or increments the current frame of the executing action
/// 
/// Arguments:
///     frame    - optional (default 1)    - The frame to set to or by
///     relative - optional (default true) - If true, frame number is incremented. If false, the frame is set directly to the given value. 
/// </summary>
public class SubactionSetFrame : Subaction {

    public override void Execute(BattleObject actor, GameAction action)
    {
        base.Execute(actor, action);
        int frame = (int) GetArgument("frame", actor, action, 1);
        bool relative = (bool)GetArgument("relative", actor, action, true);

        action.ChangeFrame(frame, relative);
    }

    public override SubactionCategory getCategory()
    {
        return SubactionCategory.CONTROL;
    }

    public override void generateDefaultArguments()
    {
        arg_list.Add(new SubactionVarData("frame", "constant", "int", "1", false));
        arg_list.Add(new SubactionVarData("relative", "constant", "bool", "true", false));
        BuildDict();
    }
}
