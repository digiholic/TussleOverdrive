using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NeutralAction : GameAction {
    public NeutralAction()
    {
        exit_action = "NeutralAction";
    }

    public override void stateTransitions()
    {
        base.stateTransitions();
        //These classes will be phased out as time goes on. Until then, we need to just exit early if we're in the builder since these don't actually use Subactions
        if (isInBuilder) return;
        StateTransitions.NeutralState(actor.GetAbstractFighter());
        StateTransitions.CheckGround(actor.GetAbstractFighter());
    }
}
