using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SubactionChangeSubimage : Subaction {
    public int subimage;

    public SubactionChangeSubimage(int _subimage)
    {
        subimage = _subimage;
    }

    public override void Execute(BattleObject obj, GameAction action)
    {
        obj.GetSpriteHandler().ChangeSubimage(subimage);
    }

    public override List<string> GetRequirements()
    {
        List<string> retList = new List<string>();
        retList.Add("SpriteHandler");
        return retList;
    }
}
