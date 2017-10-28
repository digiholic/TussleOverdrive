using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SubactionChangeSprite : Subaction
{
    public string spriteName;

    public SubactionChangeSprite(string _spriteName)
    {
        spriteName = _spriteName;
    }

    public override void Execute(BattleObject obj, GameAction action)
    {
        obj.GetSpriteHandler().ChangeSprite(spriteName);
    }

    public override List<string> GetRequirements()
    {
        List<string> retList = new List<string>();
        retList.Add("SpriteHandler");
        return retList;
    }
}