using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SubactionAccelerate : Subaction
{
    public override void Execute(BattleObject obj, GameAction action)
    {
        //Arguments
        float xFactor = (float) GetArgument("xFactor",obj,action,0);

        //Variables from fighter
        float change_x = obj.GetFloatVar(TussleConstants.MotionVariableNames.XSPEED);
        float xPref = obj.GetFloatVar(TussleConstants.MotionVariableNames.XPREF);

        //Values from settings
        float friction = Settings.current_settings.friction_ratio;
        float air_control = Settings.current_settings.aircontrol_ratio;

        if (obj.GetBoolVar(TussleConstants.FighterVariableNames.IS_GROUNDED)){
            xFactor = xFactor*friction;
        } else {
            xFactor = xFactor*air_control;
        }

        if (change_x > xPref){
            float diff = change_x - xPref;
            change_x -= Mathf.Min(diff,xFactor);
        } else if (change_x < xPref){
            float diff = xPref - change_x;
            change_x += Mathf.Min(diff,xFactor);
        }

        //Finally, update our actual speed
        obj.SendMessage("ChangeXSpeed",change_x);
    }

    public override SubactionType getSubactionType()
    {
        return SubactionType.BEHAVIOR;
    }
}
