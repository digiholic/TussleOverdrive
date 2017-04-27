using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NeutralAction : GameAction {
    public override void OnLastFrame()
    {
        base.OnLastFrame();
        actor.BroadcastMessage("DoAction","NeutralAction");
    }

    public override void stateTransitions()
    {
        base.stateTransitions();
        StateTransitions.NeutralState(actor.GetAbstractFighter());
        StateTransitions.CheckGround(actor.GetAbstractFighter());
    }
}
