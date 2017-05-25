using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Move : GameAction {
    public Move()
    {
        SetVar("direction",1);
        SetVar("accel", true);
    }
    public override void SetUp(BattleObject _actor)
    {
        base.SetUp(_actor);
        //Debug.Log("MoveAction Created");
        SetVar("direction",actor.GetIntVar("facing"));
    }

    public override void TearDown(GameAction new_action)
    {
        base.TearDown(new_action);
        if (new_action.GetVar("direction") != null)
        {
            new_action.SetVar("direction", new_action.GetIntVar("direction") * GetIntVar("direction") * actor.GetIntVar("facing"));
        }
        else if (actor.GetIntVar("facing") != GetIntVar("direction"))
                actor.BroadcastMessage("flip");
            
        actor.BroadcastMessage("ChangeXPreferred", 0.0f);
    }

    public override void Update()
    {
        base.Update();
        actor.SendMessage("ChangeXPreferred", actor.GetFloatVar("max_ground_speed") * actor.GetIntVar("facing"));

        if (((actor.GetMotionHandler().XSpeed >= -actor.GetFloatVar("max_ground_speed")) && actor.GetIntVar("facing") == -1) || 
            ((actor.GetMotionHandler().XSpeed <=  actor.GetFloatVar("max_ground_speed")) && actor.GetIntVar("facing") ==  1))
        {
            actor.GetMotionHandler().accel(actor.GetFloatVar("static_grip"));
        }

        if (actor.GetAbstractFighter().KeyHeld(InputTypeUtil.GetBackward(actor)))
        {
            SetVar("direction", -1 * actor.GetIntVar("facing"));
        }
        else
        {
            SetVar("direction", actor.GetIntVar("facing"));
        }
        
        //If direction and sprite don't match up, flip. Pretty sure this is some moonwalk stuff.
    }

    public override void stateTransitions()
    {
        base.stateTransitions();
        StateTransitions.CheckGround(actor.GetAbstractFighter());
        StateTransitions.MoveState(actor.GetAbstractFighter());
        if (current_frame > 0)
        {
            if (actor.GetAbstractFighter().KeyBuffered(InputTypeUtil.GetBackward(actor), 1, 1))
            {
                PassVariable("direction", -1 * GetIntVar("direction"));
                PassVariable("accel", false);
            }
            else if (actor.GetAbstractFighter().KeyBuffered(InputTypeUtil.GetForward(actor), 1, 1))
            {
                PassVariable("direction", GetIntVar("direction"));
                PassVariable("accel", false);
            }
        }
        //Check for dashing
        if (current_frame > last_frame)
            current_frame -= 1;
    }
}
