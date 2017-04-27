using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SubactionDeactivateHitbox : Subaction
{
    public string arguments;

    public SubactionDeactivateHitbox(string _arguments)
    {
        arguments = _arguments;
    }

    public override void Execute(BattleObject obj, GameAction action)
    {
        string[] args = arguments.Split(' ');

        string name = args[1];
        if (action.hitboxes.ContainsKey(args[1]))
            action.hitboxes[args[1]].Deactivate();
        else
            Debug.LogWarning("Current action has no hitbox names " + name);

    }

    public override List<string> GetRequirements()
    {
        List<string> retList = new List<string>();
        retList.Add("HitboxLoader");
        return retList;
    }
}