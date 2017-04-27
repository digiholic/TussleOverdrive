using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SubactionSetFrame : Subaction {
    public int frameNo;

    public SubactionSetFrame(int _frameNo)
    {
        frameNo = _frameNo;
    }

    public override void Execute(BattleObject obj, GameAction action)
    {
        action.current_frame = frameNo;
    }

    public override List<string> GetRequirements()
    {
        List<string> retList = new List<string>();
        retList.Add("ActionHandler");
        return retList;
    }
}
