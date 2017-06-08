using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StandingPivot : GameAction {
    
    public StandingPivot()
    {
        exit_action = "NeutralAction";
    }

    public override void Update()
    {
        base.Update();
        if (current_frame == 0)
            actor.SendMessage("flip");

    }
}
