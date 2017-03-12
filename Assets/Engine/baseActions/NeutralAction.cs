using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NeutralAction : GameAction {
    public override void OnLastFrame()
    {
        base.OnLastFrame();
        current_frame = 0;
    }

    public override void stateTransitions()
    {
        base.stateTransitions();
        StateTransitions.NeutralState(actor);
        StateTransitions.CheckGround(actor);
    }
}
