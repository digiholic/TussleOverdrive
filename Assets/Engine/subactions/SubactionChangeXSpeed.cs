using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SubactionChangeXSpeed : Subaction {
    public int xSpeed;
    public bool relative;

    public SubactionChangeXSpeed(int _xSpeed, bool _relative)
    {
        xSpeed = _xSpeed;
        relative = _relative;
    }

    public override void Execute(BattleObject obj, GameAction action)
    {
        if (relative)
        {
            obj.BroadcastMessage("ChangeXSpeedBy", xSpeed);
        }
        else
        {
            obj.BroadcastMessage("ChangeXSpeed", xSpeed);
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
