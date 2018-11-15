using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Stop : GameAction {
    
    public int direction = 1;

    public override void SetUp(BattleObject _actor)
    {
        base.SetUp(_actor);
        //These classes will be phased out as time goes on. Until then, we need to just exit early if we're in the builder since these don't actually use Subactions
        if (isInBuilder) return;
        //Debug.Log("StopAction Created");
        //Overrides given length if the pivot grip won't allow it
        int num_frames = Mathf.FloorToInt((actor.GetMotionHandler().XSpeed * actor.GetIntVar("facing")) / actor.GetFloatVar(TussleConstants.FighterAttributes.PIVOT_GRIP)/10);
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
            _actor.preferred_xspeed = _actor.stats['max_ground_speed']*_actor.GetAbstractFighter().facing
            _actor.accel(_actor.stats['static_grip'])
        else:*/

        //These classes will be phased out as time goes on. Until then, we need to just exit early if we're in the builder since these don't actually use Subactions
        if (isInBuilder) return;

        actor.BroadcastMessage("ChangeXPreferred", 0.0f);
        actor.GetMotionHandler().accel(actor.GetFloatVar(TussleConstants.FighterAttributes.PIVOT_GRIP));
    }

    public override void stateTransitions()
    {
        base.stateTransitions();
        //These classes will be phased out as time goes on. Until then, we need to just exit early if we're in the builder since these don't actually use Subactions
        if (isInBuilder) return;
        StateTransitions.StopState(actor.GetAbstractFighter());
        StateTransitions.CheckGround(actor.GetAbstractFighter());
        //TODO more moonwalking
        if (current_frame >= last_frame)
            actor.BroadcastMessage("DoAction","NeutralAction");
    }

    public override void TearDown(GameAction new_action)
    {
        base.TearDown(new_action);
        //Setting direction
        //if (actor.GetAbstractFighter().facing != direction)
        //    actor.flip();
    }
}
