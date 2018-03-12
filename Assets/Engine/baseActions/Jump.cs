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
            actor.SendMessage("UnRotate");
            actor.SetVar("grounded", false);
            if (actor.GetAbstractFighter().KeyHeld("Jump"))
                actor.BroadcastMessage("ChangeYSpeed", actor.GetFloatVar(TussleConstants.FighterAttributes.JUMP_HEIGHT));
            else
                actor.BroadcastMessage("ChangeYSpeed", actor.GetFloatVar(TussleConstants.FighterAttributes.SHORT_HOP_HEIGHT));

            if (Mathf.Abs(actor.GetMotionHandler().XSpeed) > actor.GetFloatVar(TussleConstants.FighterAttributes.AERIAL_TRANSITION_SPEED))
            {
                if (actor.GetMotionHandler().XSpeed < 0) //negative speed
                    actor.BroadcastMessage("ChangeXSpeed", -actor.GetFloatVar(TussleConstants.FighterAttributes.AERIAL_TRANSITION_SPEED));
                else
                    actor.BroadcastMessage("ChangeXSpeed", actor.GetFloatVar(TussleConstants.FighterAttributes.AERIAL_TRANSITION_SPEED));
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
            StateTransitions.AirState(actor.GetAbstractFighter());
        if (current_frame > last_frame)
            actor.BroadcastMessage("DoAction", "Fall");
    }
}
