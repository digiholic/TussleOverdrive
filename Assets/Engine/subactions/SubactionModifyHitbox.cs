using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SubactionModifyHitbox : Subaction
{
    public string arguments;

    public SubactionModifyHitbox(string _arguments)
    {
        arguments = _arguments;
    }

    public override void Execute(BattleObject obj, GameAction action)
    {
        string[] args = arguments.Split(' ');

        string name = args[1];
        Dictionary<string,string> hbox_dict = new Dictionary<string, string>();
        for (int i = 2; i < args.Length; i = i + 2)
        {
            hbox_dict[args[i]] = args[i + 1];
        }
        if (action.hitboxes.ContainsKey(name))
        {
            action.hitboxes[name].LoadValuesFromDict(obj.GetAbstractFighter(), hbox_dict);
        }

    }

    public override List<string> GetRequirements()
    {
        List<string> retList = new List<string>();
        retList.Add("HitboxLoader");
        return retList;
    }
}
