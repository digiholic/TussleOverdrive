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
            actor.SetVar("grounded", false);
            if (actor.GetAbstractFighter().GetControllerButton("Jump"))
                actor.BroadcastMessage("ChangeYSpeed", actor.GetFloatVar("jump_height"));
            else
                actor.BroadcastMessage("ChangeYSpeed", actor.GetFloatVar("short_hop_height"));

            if (Mathf.Abs(actor.GetMotionHandler().XSpeed) > actor.GetFloatVar("aerial_transition_speed"))
            {
                if (actor.GetMotionHandler().XSpeed < 0) //negative speed
                    actor.BroadcastMessage("ChangeXSpeed", -actor.GetFloatVar("aerial_transition_speed"));
                else
                    actor.BroadcastMessage("ChangeXSpeed", actor.GetFloatVar("aerial_transition_speed"));
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
            StateTransitions.JumpState(actor.GetAbstractFighter());
        if (current_frame > jump_frame)
            StateTransitions.AirControl(actor.GetAbstractFighter());
        if (current_frame > last_frame)
            actor.BroadcastMessage("DoAction", "Fall");
    }
}
