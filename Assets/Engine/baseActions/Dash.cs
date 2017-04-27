using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dash : GameAction {

    public int run_start_frame = 0;
    public int direction = 1;
    public bool accel = true;

    public override void TearDown(GameAction new_action)
    {
        base.TearDown(new_action);
        //if next action has direction, set it
        if (actor.GetAbstractFighter().facing != direction)
            actor.GetAbstractFighter().flip();
        actor.BroadcastMessage("ChangeXPreferred", 0.0f);
    }

    public override void Update()
    {
        base.Update();
        if (current_frame == 0)
            actor.BroadcastMessage("ChangeXPreferred", actor.GetAbstractFighter().run_speed * actor.GetAbstractFighter().facing);
        StateTransitions.CheckGround(actor.GetAbstractFighter());
        if ((actor.GetAbstractFighter().GetControllerAxis("Horizontal") * actor.GetAbstractFighter().facing) < 0.0f) //If you are holding the opposite direction of movement
            direction = actor.GetAbstractFighter().facing * -1;
        else
            direction = actor.GetAbstractFighter().facing;

        if (current_frame > last_frame)
            //current_frame = run_start_frame;
            //VERY TODO UNTIL VARIABLES IN ACTION
            current_frame = last_frame;
    }

    public override void stateTransitions()
    {
        base.stateTransitions();
        StateTransitions.DashState(actor.GetAbstractFighter());
    }
}
