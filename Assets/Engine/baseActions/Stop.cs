using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Stop : Action {
    
    public int direction = 1;

    public override void SetUp(AbstractFighter _actor)
    {
        length = 9;
        sprite_name = "pivot";
        sprite_rate = 3;
        base.SetUp(_actor);
        //Debug.Log("StopAction Created");
        //Overrides given length if the pivot grip won't allow it
        int num_frames = Mathf.FloorToInt((actor._xSpeed * actor.facing) / actor.pivot_grip);
        if (num_frames < last_frame)
            current_frame = Mathf.Min(last_frame - num_frames, last_frame - 1);
        else
            last_frame = num_frames;
    }

    public override void Update()
    {
        base.Update();
        //Do we need moonwalking?
        /* if _actor.keysContain(key) and _actor.keysContain(invkey):
            _actor.preferred_xspeed = _actor.stats['max_ground_speed']*_actor.facing
            _actor.accel(_actor.stats['static_grip'])
        else:*/
        actor._xPreferred = 0;
        actor.accel(actor.pivot_grip);
    }

    public override void stateTransitions()
    {
        base.stateTransitions();
        StateTransitions.StopState(actor);
        StateTransitions.CheckGround(actor);
        //TODO more moonwalking
        if (current_frame >= last_frame)
            actor.doAction("NeutralAction");
    }

    public override void TearDown(Action new_action)
    {
        base.TearDown(new_action);
        //Setting direction
        //if (actor.facing != direction)
        //    actor.flip();
    }
}
