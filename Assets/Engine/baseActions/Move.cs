using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Move : GameAction {

    public bool accel = true;
    public int direction = 1;

    public override void SetUp(BattleObject _actor)
    {
        base.SetUp(_actor);
        //Debug.Log("MoveAction Created");
        direction = actor.GetIntVar("facing");
    }

    public override void TearDown(GameAction new_action)
    {
        base.TearDown(new_action);
        //TODO Next action direction? Do we still need this?
        if (actor.GetIntVar("facing") != direction)
            actor.BroadcastMessage("flip");
        actor.BroadcastMessage("ChangeXPreferred", 0.0f);
    }

    public override void Update()
    {
        base.Update();
        actor.BroadcastMessage("ChangeXPreferred", actor.GetFloatVar("max_ground_speed") * direction);
        if (((actor.GetMotionHandler().XSpeed >= -actor.GetFloatVar("max_ground_speed")) && actor.GetIntVar("facing") == -1) || 
            ((actor.GetMotionHandler().XSpeed <=  actor.GetFloatVar("max_ground_speed")) && actor.GetIntVar("facing") ==  1))
        {
            actor.GetMotionHandler().accel(actor.GetFloatVar("static_grip"));
        }
        if ((actor.GetAbstractFighter().GetControllerAxis("Horizontal") * actor.GetIntVar("facing")) < 0.0f) //If you are holding the opposite direction of movement
            direction = actor.GetIntVar("facing") * -1;
        else
            direction = actor.GetIntVar("facing");
        //If direction and sprite don't match up, flip. Pretty sure this is some moonwalk stuff.
    }

    public override void stateTransitions()
    {
        base.stateTransitions();
        StateTransitions.CheckGround(actor.GetAbstractFighter());
        StateTransitions.MoveState(actor.GetAbstractFighter());
        //Check for dashing
        if (current_frame > last_frame)
            current_frame -= 1;
    }
}
