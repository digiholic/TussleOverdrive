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
        if (actor.facing != direction)
            actor.flip();
        actor._xPreferred = 0;
    }

    public override void Update()
    {
        base.Update();
        if (current_frame == 0)
            actor._xPreferred = actor.run_speed * actor.facing;
        StateTransitions.CheckGround(actor);
        if ((actor.GetControllerAxis("Horizontal") * actor.facing) < 0.0f) //If you are holding the opposite direction of movement
            direction = actor.facing * -1;
        else
            direction = actor.facing;

        if (current_frame > last_frame)
            //current_frame = run_start_frame;
            //VERY TODO UNTIL VARIABLES IN ACTION
            current_frame = last_frame;
    }

    public override void stateTransitions()
    {
        base.stateTransitions();
        StateTransitions.DashState(actor);
    }
}
