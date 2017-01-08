using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NeutralAction : Action {
    public override void SetUp(AbstractFighter _actor)
    {
        sprite_name = "idle";
        base.SetUp(_actor);
        //Debug.Log("NeutralAction Created");
    }

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
