using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SubactionIfButtonBuffered : Subaction
{
    public override void Execute(BattleObject obj, GameAction action)
    {
        string button = (string) GetArgument("button",obj,action);
        int bufferWindow = (int) GetArgument("bufferWindow",obj,action,0);
        bool pressed = (bool) GetArgument("pressed",obj,action,true);

        bool value = obj.GetInputBuffer().KeyBuffered(button,bufferWindow,pressed);
        
        action.cond_list.Add(value);
        action.cond_depth++;
    }

    public override SubactionType getSubactionType()
    {
        return SubactionType.CONTROL;
    }

    public override bool isConditional()
    {
        return true;
    }
}
