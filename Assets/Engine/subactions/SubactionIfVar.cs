using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SubactionIfVar : Subaction {
    public string source;
    public string name;
    public string comparison;
    public string value;

    public SubactionIfVar(string _source, string _name, string _comparison, string value)
    {

    }

    public override void Execute(BattleObject obj, GameAction action)
    {
        //TODO
        action.cond_list.Add(true);
        action.cond_depth++;
    }

    public override List<string> GetRequirements()
    {
        List<string> retList = new List<string>();
        retList.Add("ActionHandler");
        return retList;
    }
}
