using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SubactionSetAnchor : Subaction
{
    public string name;
    public int centerx;
    public int centery;

    public SubactionSetAnchor(string _name, int _centerx, int _centery)
    {
        name = _name;
        centerx = _centerx;
        centery = _centery;
    }

    public override void Execute(BattleObject obj, GameAction action)
    {
        obj.GetAnchorPoint(name).MoveAnchorPixel(centerx, centery);
    }

    public override List<string> GetRequirements()
    {
        List<string> retList = new List<string>();
        retList.Add("SpriteHandler");
        return retList;
    }
}