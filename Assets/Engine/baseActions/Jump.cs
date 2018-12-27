using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Jump : GameAction
{
    private int jump_frame = 3;
    
    public override void Update()
    {
        base.Update();
        //These classes will be phased out as time goes on. Until then, we need to just exit early if we're in the builder since these don't actually use Subactions
        if (isInBuilder) return;
        //These classes will be phased out as time goes on. Until then, we need to just exit early if we're in the builder since these don't actually use Subactions
        if (isInBuilder) return;
        if (current_frame == jump_frame)
        {
            actor.SendMessage("UnRotate");
            actor.SetVar(TussleConstants.FighterVariableNames.IS_GROUNDED, false);
            if (actor.GetAbstractFighter().KeyHeld("Jump"))
                actor.BroadcastMessage("ChangeYSpeed", actor.GetFloatVar(TussleConstants.FighterAttributes.JUMP_HEIGHT));
            else
                actor.BroadcastMessage("ChangeYSpeed", actor.GetFloatVar(TussleConstants.FighterAttributes.SHORT_HOP_HEIGHT));

            if (Mathf.Abs(actor.GetFloatVar(TussleConstants.MotionVariableNames.XSPEED)) > actor.GetFloatVar(TussleConstants.FighterAttributes.AERIAL_TRANSITION_SPEED))
            {
                if (actor.GetFloatVar(TussleConstants.MotionVariableNames.XSPEED) < 0) //negative speed
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
        //These classes will be phased out as time goes on. Until then, we need to just exit early if we're in the builder since these don't actually use Subactions
        if (isInBuilder) return;
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
