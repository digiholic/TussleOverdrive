using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SubactionIfButtonHeld : Subaction
{
    public override void Execute(BattleObject obj, GameAction action)
    {
        string button = (string) GetArgument("button",obj,action);
        bool pressed = (bool) GetArgument("pressed",obj,action,true);

        bool buttonHeld = obj.GetInputBuffer().GetKey(button);

        //GetKey returns true if the button is held. If we're looking for a release (pressed is false) we need to invert it
        bool value;
        if (pressed) value = buttonHeld;
        else value = !buttonHeld;

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