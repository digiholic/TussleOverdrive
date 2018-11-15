using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseAttack : GameAction {

    public override void TearDown(GameAction new_action)
    {
        base.TearDown(new_action);
        foreach (KeyValuePair<string,Hitbox> entry in hitboxes)
        {
            GameObject.Destroy(entry.Value.gameObject);
        }
    }

    public override void stateTransitions()
    {
        base.stateTransitions();
        //These classes will be phased out as time goes on. Until then, we need to just exit early if we're in the builder since these don't actually use Subactions
        if (isInBuilder) return;
        if (current_frame >= last_frame)
            if (actor.GetBoolVar("grounded"))
                actor.BroadcastMessage("DoAction", "NeutralAction");
            else
                actor.BroadcastMessage("DoAction", "Fall");
    }
}

public class AirAttack : GameAction
{

    public override void TearDown(GameAction new_action)
    {
        base.TearDown(new_action);
        foreach (KeyValuePair<string, Hitbox> entry in hitboxes)
        {
            GameObject.Destroy(entry.Value.gameObject);
        }
    }

    public override void stateTransitions()
    {
        base.stateTransitions();
        //These classes will be phased out as time goes on. Until then, we need to just exit early if we're in the builder since these don't actually use Subactions
        if (isInBuilder) return;
        actor.SendMessage("CheckForGround");
        if (current_frame >= last_frame)
            if (actor.GetBoolVar("grounded"))
                actor.BroadcastMessage("DoAction", "NeutralAction");
            else
                actor.BroadcastMessage("DoAction", "Fall");
        if (actor.GetBoolVar("grounded"))
            actor.BroadcastMessage("DoAction", "Land");
    }
}