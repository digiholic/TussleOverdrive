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
        StateTransitions.NeutralState(actor.GetAbstractFighter());
        StateTransitions.CheckGround(actor.GetAbstractFighter());
    }
}
