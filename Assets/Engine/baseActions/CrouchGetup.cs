using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CrouchGetup : GameAction {
    public override void TearDown(GameAction new_action)
    {
        base.TearDown(new_action);
        //Delete crouch cancel armor
    }

    public override void stateTransitions()
    {
        base.stateTransitions();
        StateTransitions.CrouchState(actor.GetAbstractFighter());
        StateTransitions.CheckGround(actor.GetAbstractFighter());
        //TODO platform phase
        if (current_frame >= last_frame)
            actor.BroadcastMessage("DoAction", "NeutralAction");
    }

    public override void Update()
    {
        base.Update();
        actor.BroadcastMessage("ChangeXPreferred", 0.0f);
    }
}
