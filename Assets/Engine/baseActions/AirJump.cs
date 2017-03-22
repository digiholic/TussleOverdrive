using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AirJump : GameAction {

    private int jump_frame = 6;

    public override void Update()
    {
        base.Update();
        if (current_frame < jump_frame)
        {
            actor._ySpeed = 0;
        }
        if (current_frame == jump_frame)
        {
            actor._ySpeed = actor.air_jump_height;
            actor.jumps--;
        }
    }

    public override void stateTransitions()
    {
        base.stateTransitions();
        //if (actor.GetControllerButton("Attack")) //&& actor.CheckSmash("Up")
        //actor.doAction("UpSmash")
        //if (actor.GetControllerButton("Special")) //&& actor.CheckSmash("Up")
        //actor.doAction("UpSpecial")
        if (current_frame > jump_frame)
            StateTransitions.AirState(actor);
        if (current_frame > last_frame)
            actor.doAction("Fall");

    }
}
