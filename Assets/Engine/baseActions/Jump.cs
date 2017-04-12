using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Jump : GameAction
{
    private int jump_frame = 3;
    
    public override void Update()
    {
        base.Update();
        
        if (current_frame == jump_frame)
        {
            if (actor.GetControllerButton("Jump"))
                actor.battleObject.YSpeed = actor.jump_height;
            else
                actor.battleObject.YSpeed = actor.short_hop_height;

            if (Mathf.Abs(actor.battleObject.XSpeed) > actor.aerial_transition_speed)
            {
                if (actor.battleObject.XSpeed < 0) //negative speed
                    actor.battleObject.XSpeed = -actor.aerial_transition_speed;
                else
                    actor.battleObject.XSpeed = actor.aerial_transition_speed;
            }
            //actor.ChangeSprite("jump");
        }
    }

    public override void stateTransitions()
    {
        base.stateTransitions();
        //if (actor.GetControllerButton("Shield"))
        //actor.doAction("AirDodge");
        //if (actor.GetControllerButton("Attack")) //&& actor.CheckSmash("Up")
        //actor.doAction("UpSmash")
        //if (actor.GetControllerButton("Special")) //&& actor.CheckSmash("Up")
        //actor.doAction("UpSpecial")
        if (current_frame <= jump_frame)
            StateTransitions.JumpState(actor);
        if (current_frame > jump_frame)
            StateTransitions.AirState(actor);
        if (current_frame > last_frame)
            actor.doAction("Fall");

    }
}
