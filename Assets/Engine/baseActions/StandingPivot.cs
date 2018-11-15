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
        //These classes will be phased out as time goes on. Until then, we need to just exit early if we're in the builder since these don't actually use Subactions
        if (isInBuilder) return;
        if (current_frame == 0)
            actor.SendMessage("flip");

    }
}
