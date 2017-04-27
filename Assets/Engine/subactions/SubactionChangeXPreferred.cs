using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SubactionChangeXPreferred : Subaction
{
    public int xPreferred;
    public bool relative;

    public SubactionChangeXPreferred(int _xPreferred, bool _relative)
    {
        xPreferred = _xPreferred;
        relative = _relative;
    }

    public override void Execute(BattleObject obj, GameAction action)
    {
        if (relative)
        {
            obj.BroadcastMessage("ChangeXPreferredBy", xPreferred);
        }
        else
        {
            obj.BroadcastMessage("ChangeXPreferred", xPreferred);
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