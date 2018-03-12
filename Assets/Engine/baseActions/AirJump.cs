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
            actor.BroadcastMessage("ChangeYSpeed", 0.0f);
        }
        if (current_frame == jump_frame)
        {
            actor.BroadcastMessage("ChangeYSpeed", actor.GetFloatVar(TussleConstants.FighterAttributes.AIR_JUMP_HEIGHT));
            actor.SetVar(TussleConstants.FighterAttributes.JUMPS, actor.GetIntVar(TussleConstants.FighterAttributes.JUMPS) - 1);
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
            StateTransitions.AirState(actor.GetAbstractFighter());
        if (current_frame > last_frame)
            actor.BroadcastMessage("DoAction", "Fall");

    }
}
