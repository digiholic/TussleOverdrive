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
        //These classes will be phased out as time goes on. Until then, we need to just exit early if we're in the builder since these don't actually use Subactions
        if (isInBuilder) return;
        //Debug.Log("MoveAction Created");
        SetVar("direction",actor.GetIntVar(TussleConstants.FighterVariableNames.FACING_DIRECTION));
    }

    public override void TearDown(GameAction new_action)
    {
        base.TearDown(new_action);
        //These classes will be phased out as time goes on. Until then, we need to just exit early if we're in the builder since these don't actually use Subactions
        if (isInBuilder) return;
        if (new_action.HasVar("direction"))
        {
            new_action.SetVar("direction", new_action.GetIntVar("direction") * GetIntVar("direction") * actor.GetIntVar(TussleConstants.FighterVariableNames.FACING_DIRECTION));
        }
        else if (actor.GetIntVar(TussleConstants.FighterVariableNames.FACING_DIRECTION) != GetIntVar("direction"))
                actor.BroadcastMessage("flip");
            
        actor.BroadcastMessage("ChangeXPreferred", 0.0f, SendMessageOptions.RequireReceiver);
    }

    public override void Update()
    {
        base.Update();
        //These classes will be phased out as time goes on. Until then, we need to just exit early if we're in the builder since these don't actually use Subactions
        if (isInBuilder) return;
        actor.SendMessage("ChangeXPreferred", actor.GetFloatVar(TussleConstants.FighterAttributes.MAX_GROUND_SPEED) * actor.GetIntVar(TussleConstants.FighterVariableNames.FACING_DIRECTION));

        if (((actor.GetFloatVar(TussleConstants.MotionVariableNames.XSPEED) >= -actor.GetFloatVar(TussleConstants.FighterAttributes.MAX_GROUND_SPEED)) && actor.GetIntVar(TussleConstants.FighterVariableNames.FACING_DIRECTION) == -1) || 
            ((actor.GetFloatVar(TussleConstants.MotionVariableNames.XSPEED) <=  actor.GetFloatVar(TussleConstants.FighterAttributes.MAX_GROUND_SPEED)) && actor.GetIntVar(TussleConstants.FighterVariableNames.FACING_DIRECTION) ==  1))
        {
            actor.SendMessage("accel", actor.GetFloatVar(TussleConstants.FighterAttributes.STATIC_GRIP));
        }
        if (actor.GetInputBuffer().DirectionHeld("Backward"))
        {
            SetVar("direction", -1 * actor.GetIntVar(TussleConstants.FighterVariableNames.FACING_DIRECTION));
        }
        else
        {
            SetVar("direction", actor.GetIntVar(TussleConstants.FighterVariableNames.FACING_DIRECTION));
        }
        
        //If direction and sprite don't match up, flip. Pretty sure this is some moonwalk stuff.
    }

    public override void stateTransitions()
    {
        base.stateTransitions();
        //These classes will be phased out as time goes on. Until then, we need to just exit early if we're in the builder since these don't actually use Subactions
        if (isInBuilder) return;
        StateTransitions.CheckGround(actor.GetAbstractFighter());
        StateTransitions.MoveState(actor.GetAbstractFighter());
        if (current_frame > 0)
        {
            if (actor.GetInputBuffer().DirectionHeld("Backward"))
            {
                PassVariable("direction", -1 * GetIntVar("direction"));
                PassVariable("accel", false);
            }
            else if (actor.GetInputBuffer().DirectionHeld("Forward"))
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
