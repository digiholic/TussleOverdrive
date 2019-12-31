using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dash : GameAction {
    public Dash()
    {
        SetVar("run_start_frame", 1);
        SetVar("direction", 1);
        SetVar("accel", true);
    }

    public override void TearDown(GameAction new_action)
    {
        //These classes will be phased out as time goes on. Until then, we need to just exit early if we're in the builder since these don't actually use Subactions
        if (isInBuilder) return;
        if (new_action.HasVar("direction"))
        {
            new_action.SetVar("direction", GetIntVar("direction") * actor.GetIntVar("direction"));
        } else
        {
            //_actor.facing = self.direction
            //if (_actor.facing == 1 and _actor.sprite.flip == "left") or (_actor.facing == -1 and _actor.sprite.flip == "right"):
            //_actor.sprite.flipX()
        }
        
        if (actor.GetIntVar(TussleConstants.FighterVariableNames.FACING_DIRECTION) != GetIntVar("direction"))
            actor.SendMessage("flip");
        actor.BroadcastMessage("ChangeXPreferred", 0.0f, SendMessageOptions.RequireReceiver);
        base.TearDown(new_action);
    }

    public override void Update()
    {
        //These classes will be phased out as time goes on. Until then, we need to just exit early if we're in the builder since these don't actually use Subactions
        if (isInBuilder) return;
        if (current_frame == GetIntVar("run_start_frame"))
        {
            actor.SendMessage("ChangeXPreferred", actor.GetFloatVar(TussleConstants.FighterAttributes.RUN_SPEED) * actor.GetIntVar(TussleConstants.FighterVariableNames.FACING_DIRECTION));
            
        }
        
        if (current_frame > last_frame)
        {
            //current_frame = run_start_frame;
            //VERY TODO UNTIL VARIABLES IN ACTION
            current_frame = GetIntVar("run_start_frame");
        }
        base.Update();
    }

    public override void stateTransitions()
    {
        base.stateTransitions();
        //These classes will be phased out as time goes on. Until then, we need to just exit early if we're in the builder since these don't actually use Subactions
        if (isInBuilder) return;
        StateTransitions.DashState(actor.GetAbstractFighter());
    }
}
