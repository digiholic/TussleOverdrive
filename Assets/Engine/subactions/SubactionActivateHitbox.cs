using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SubactionActivateHitbox : Subaction
{
    public string name;
    public int life;

    public SubactionActivateHitbox(string _name, int _life)
    {
        name = _name;
        life = _life;
    }

    public override void Execute(BattleObject obj, GameAction action)
    {
        if (action.hitboxes.ContainsKey(name))
            action.hitboxes[name].Activate(life);
        else
            Debug.LogWarning("Current action has no hitbox named " + name);
    }

    public override List<string> GetRequirements()
    {
        List<string> retList = new List<string>();
        retList.Add("HitboxLoader");
        return retList;
    }
}