using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fall : GameAction
{
    public Fall()
    {
        exit_action = "Fall";
    }

    public override void stateTransitions()
    {
        base.stateTransitions();
        //These classes will be phased out as time goes on. Until then, we need to just exit early if we're in the builder since these don't actually use Subactions
        if (isInBuilder) return;
        StateTransitions.AirState(actor.GetAbstractFighter());
        StateTransitions.CheckLedges(actor.GetAbstractFighter());
    }

    public override void SetUp(BattleObject obj)
    {
        base.SetUp(obj);
        //These classes will be phased out as time goes on. Until then, we need to just exit early if we're in the builder since these don't actually use Subactions
        if (isInBuilder) return;
        actor.SendMessage("UnRotate");
    }
}