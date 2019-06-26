using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AirJump : GameAction {

    private int jump_frame = 6;

    public override void Update()
    {
        base.Update();
        //These classes will be phased out as time goes on. Until then, we need to just exit early if we're in the builder since these don't actually use Subactions
        if (isInBuilder) return;
        if (current_frame < jump_frame)
        {
            actor.BroadcastMessage("ChangeYSpeed", 0.0f, SendMessageOptions.RequireReceiver);
        }
        if (current_frame == jump_frame)
        {
            actor.BroadcastMessage("ChangeYSpeed", actor.GetFloatVar(TussleConstants.FighterAttributes.AIR_JUMP_HEIGHT));
            actor.SetVar(TussleConstants.FighterVariableNames.JUMPS_REMAINING, actor.GetIntVar(TussleConstants.FighterVariableNames.JUMPS_REMAINING) - 1);
        }
    }

    public override void stateTransitions()
    {
        base.stateTransitions();
        //These classes will be phased out as time goes on. Until then, we need to just exit early if we're in the builder since these don't actually use Subactions
        if (isInBuilder) return;
        //if (actor.GetControllerButton("Attack")) //&& actor.CheckSmash("Up")
        //actor.doAction("UpSmash")
        //if (actor.GetControllerButton("Special")) //&& actor.CheckSmash("Up")
        //actor.doAction("UpSpecial")
        if (current_frame > jump_frame)
            StateTransitions.AirState(actor.GetAbstractFighter());
        if (current_frame > last_frame)
            actor.BroadcastMessage("DoAction", "Fall");

    }
}
