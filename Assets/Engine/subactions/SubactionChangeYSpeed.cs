using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SubactionChangeYSpeed : Subaction
{
    public int ySpeed;
    public bool relative;

    public SubactionChangeYSpeed(int _ySpeed, bool _relative)
    {
        ySpeed = _ySpeed;
        relative = _relative;
    }

    public override void Execute(BattleObject obj, GameAction action)
    {
        if (relative)
        {
            obj.BroadcastMessage("ChangeYSpeedBy", ySpeed);
        }
        else
        {
            obj.BroadcastMessage("ChangeYSpeed", ySpeed);
        }
    }

    public override List<string> GetRequirements()
    {
        List<string> retList = new List<string>();
        retList.Add("ActionHandler");
        retList.Add("MotionHandler");
        return retList;
    }
}