using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dash : GameAction {

    public int run_start_frame = 0;
    public int direction = 1;
    public bool accel = true;

    public Dash()
    {
        SetVar("run_start_frame", 0);
        SetVar("direction", 1);
        SetVar("accel", true);
    }

    public override void TearDown(GameAction new_action)
    {
        base.TearDown(new_action);
        if (new_action.HasVar("direction"))
        {
            new_action.SetVar("direction", GetIntVar("direction") * actor.GetIntVar("direction"));
        } else
        {
            //_actor.facing = self.direction
            //if (_actor.facing == 1 and _actor.sprite.flip == "left") or (_actor.facing == -1 and _actor.sprite.flip == "right"):
            //_actor.sprite.flipX()
        }
        
        if (actor.GetIntVar("facing") != direction)
            actor.SendMessage("flip");
        actor.BroadcastMessage("ChangeXPreferred", 0.0f);
    }

    public override void Update()
    {
        base.Update();
        if (current_frame == 0)
            actor.BroadcastMessage("ChangeXPreferred", actor.GetFloatVar("run_speed") * actor.GetIntVar("facing"));
        StateTransitions.CheckGround(actor.GetAbstractFighter());
        if ((actor.GetAbstractFighter().GetControllerAxis("Horizontal") * actor.GetIntVar("facing")) < 0.0f) //If you are holding the opposite direction of movement
            direction = actor.GetIntVar("facing") * -1;
        else
            direction = actor.GetIntVar("facing");

        if (current_frame > last_frame)
            //current_frame = run_start_frame;
            //VERY TODO UNTIL VARIABLES IN ACTION
            current_frame = GetIntVar("run_start_frame");
    }

    public override void stateTransitions()
    {
        base.stateTransitions();
        StateTransitions.DashState(actor.GetAbstractFighter());
    }
}
