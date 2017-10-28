using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SubactionChangeAnim : Subaction {
    public string animName;

    public SubactionChangeAnim(string _animName)
    {
        animName = _animName;
    }

    public override void Execute(BattleObject obj, GameAction action)
    {
        //obj.GetModelHandler().ChangeAnimation(animName);
    }

    public override List<string> GetRequirements()
    {
        List<string> retList = new List<string>();
        retList.Add("ModelHandler");
        return retList;
    }
}