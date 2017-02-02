using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fall : GameAction
{

    // Use this for initialization
    public override void SetUp(AbstractFighter _actor)
    {
        sprite_name = "jump";
        base.SetUp(_actor);
        actor._xPreferred = 0;
        actor._yPreferred = actor.max_fall_speed;
        //Debug.Log("FallAction Created");
    }
    
    public override void stateTransitions()
    {
        base.stateTransitions();
        StateTransitions.AirState(actor);
        StateTransitions.CheckLedges(actor);
    }
}