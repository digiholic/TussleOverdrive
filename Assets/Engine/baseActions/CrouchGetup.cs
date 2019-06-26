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
        //These classes will be phased out as time goes on. Until then, we need to just exit early if we're in the builder since these don't actually use Subactions
        if (isInBuilder) return;
        StateTransitions.CrouchState(actor.GetAbstractFighter());
        StateTransitions.CheckGround(actor.GetAbstractFighter());
        //TODO platform phase
        if (current_frame >= last_frame)
            actor.BroadcastMessage("DoAction", "NeutralAction");
    }

    public override void Update()
    {
        base.Update();
        //These classes will be phased out as time goes on. Until then, we need to just exit early if we're in the builder since these don't actually use Subactions
        if (isInBuilder) return;
        actor.BroadcastMessage("ChangeXPreferred", 0.0f, SendMessageOptions.RequireReceiver);
    }
}
