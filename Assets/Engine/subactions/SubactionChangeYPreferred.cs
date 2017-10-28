using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SubactionChangeYPreferred : Subaction
{
    public int yPreferred;
    public bool relative;

    public SubactionChangeYPreferred(int _yPreferred, bool _relative)
    {
        yPreferred = _yPreferred;
        relative = _relative;
    }

    public override void Execute(BattleObject obj, GameAction action)
    {
        if (relative)
        {
            obj.BroadcastMessage("ChangeYPreferredBy", yPreferred);
        }
        else
        {
            obj.BroadcastMessage("ChangeYPreferred", yPreferred);
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