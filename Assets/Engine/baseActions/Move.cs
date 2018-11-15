﻿using System.Collections;
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
        SetVar("direction",actor.GetIntVar("facing"));
    }

    public override void TearDown(GameAction new_action)
    {
        base.TearDown(new_action);
        //These classes will be phased out as time goes on. Until then, we need to just exit early if we're in the builder since these don't actually use Subactions
        if (isInBuilder) return;
        if (new_action.HasVar("direction"))
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
        //These classes will be phased out as time goes on. Until then, we need to just exit early if we're in the builder since these don't actually use Subactions
        if (isInBuilder) return;
        actor.SendMessage("ChangeXPreferred", actor.GetFloatVar(TussleConstants.FighterAttributes.MAX_GROUND_SPEED) * actor.GetIntVar("facing"));

        if (((actor.GetMotionHandler().XSpeed >= -actor.GetFloatVar(TussleConstants.FighterAttributes.MAX_GROUND_SPEED)) && actor.GetIntVar("facing") == -1) || 
            ((actor.GetMotionHandler().XSpeed <=  actor.GetFloatVar(TussleConstants.FighterAttributes.MAX_GROUND_SPEED)) && actor.GetIntVar("facing") ==  1))
        {
            actor.GetMotionHandler().accel(actor.GetFloatVar(TussleConstants.FighterAttributes.STATIC_GRIP));
        }
        if (actor.GetInputBuffer().DirectionHeld("Backward"))
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
