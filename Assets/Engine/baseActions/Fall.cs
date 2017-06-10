using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fall : GameAction
{
    public Fall()
    {
        exit_action = "Fall";
    }

    public override void stateTransitions()
    {
        base.stateTransitions();
        StateTransitions.AirState(actor.GetAbstractFighter());
        StateTransitions.CheckLedges(actor.GetAbstractFighter());
    }
}