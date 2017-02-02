using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CrouchGetup : GameAction {

    public override void SetUp(AbstractFighter _actor)
    {
        sprite_name = "land";
        length = 6;
        sprite_rate = -2;
        base.SetUp(_actor);
        //Debug.Log("CrouchGetupAction Created");
    }

    public override void TearDown(GameAction new_action)
    {
        base.TearDown(new_action);
        //Delete crouch cancel armor
    }

    public override void stateTransitions()
    {
        base.stateTransitions();
        StateTransitions.CrouchState(actor);
        StateTransitions.CheckGround(actor);
        //TODO platform phase
        if (current_frame >= last_frame)
            actor.doAction("NeutralAction");
    }

    public override void Update()
    {
        base.Update();
        actor._xPreferred = 0;
    }
}
