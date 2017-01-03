using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AirJump : Action {

    private int jump_frame = 6;

    public override void SetUp(AbstractFighter _actor)
    {
        sprite_name = "airjump";
        sprite_rate = 2;
        length = 8;
        base.SetUp(_actor);
        Debug.Log("AirJumpAction Created");
    }

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
        //if (Input.GetButton("Attack")) //&& actor.CheckSmash("Up")
        //actor.doAction("UpSmash")
        //if (Input.GetButton("Special")) //&& actor.CheckSmash("Up")
        //actor.doAction("UpSpecial")
        StateTransitions.JumpState(actor);
        if (current_frame > last_frame)
            actor.doAction("Fall");

    }
}
