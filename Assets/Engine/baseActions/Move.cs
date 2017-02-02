using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Move : GameAction {

    public bool accel = true;
    public int direction = 1;

    public override void SetUp(AbstractFighter _actor)
    {
        length = 15;
        sprite_name = "run";
        sprite_rate = 3;
        loop = false;
        base.SetUp(_actor);
        //Debug.Log("MoveAction Created");
        direction = actor.facing;
    }

    public override void TearDown(GameAction new_action)
    {
        base.TearDown(new_action);
        //TODO Next action direction? Do we still need this?
        if (actor.facing != direction)
            actor.flip();
        actor._xPreferred = 0;
    }

    public override void Update()
    {
        base.Update();
        actor._xPreferred = actor.max_ground_speed * direction;
        if (((actor._xSpeed >= -actor.max_ground_speed) && actor.facing == -1) || 
            ((actor._xSpeed <=  actor.max_ground_speed) && actor.facing ==  1))
        {
            actor.accel(actor.static_grip);
        }
        if ((actor.GetControllerAxis("Horizontal") * actor.facing) < 0.0f) //If you are holding the opposite direction of movement
            direction = actor.facing * -1;
        else
            direction = actor.facing;
        //If direction and sprite don't match up, flip. Pretty sure this is some moonwalk stuff.
    }

    public override void stateTransitions()
    {
        base.stateTransitions();
        StateTransitions.CheckGround(actor);
        StateTransitions.MoveState(actor);
        //Check for dashing
        if (current_frame > last_frame)
            current_frame -= 1;
    }
}
