using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SubactionDoTransition : Subaction {
    private string transitionName;

    public SubactionDoTransition(string _transitionName)
    {
        transitionName = _transitionName;
    }

    public override void Execute(BattleObject obj, GameAction action)
    {
        //obj.BroadcastMessage("DoAction", actionName);
        StateTransitions.LoadTransitionState(transitionName, obj.GetAbstractFighter());
    }

    public override List<string> GetRequirements()
    {
        List<string> retList = new List<string>();
        retList.Add("ActionHandler");
        retList.Add("AbstractFighter");
        return retList;
    }
}
