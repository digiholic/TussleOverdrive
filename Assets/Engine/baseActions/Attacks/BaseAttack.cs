using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseAttack : GameAction {

    public override void TearDown(GameAction new_action)
    {
        base.TearDown(new_action);
        foreach (KeyValuePair<string,Hitbox> entry in hitboxes)
        {
            Destroy(entry.Value.gameObject);
        }
    }

    public override void stateTransitions()
    {
        base.stateTransitions();
        StateTransitions.CheckGround(actor);
        if (current_frame >= last_frame)
            if(actor.grounded)
                actor.doAction("NeutralAction");
            else
                actor.doAction("Fall");
    }
}

public class AirAttack : GameAction
{

    public Dictionary<string, Hitbox> hitboxes = new Dictionary<string, Hitbox>();

    public override void TearDown(GameAction new_action)
    {
        base.TearDown(new_action);
        foreach (KeyValuePair<string, Hitbox> entry in hitboxes)
        {
            Destroy(entry.Value.gameObject);
        }
    }

    public override void stateTransitions()
    {
        base.stateTransitions();
        if (current_frame >= last_frame)
            if (actor.grounded)
                actor.doAction("NeutralAction");
            else
                actor.doAction("Fall");
        if (actor.grounded)
            actor.doAction("Land");
    }
}