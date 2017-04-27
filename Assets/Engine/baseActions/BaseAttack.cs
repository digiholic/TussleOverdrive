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
        StateTransitions.CheckGround(actor.GetAbstractFighter());
        if (current_frame >= last_frame)
            if (actor.GetAbstractFighter().grounded)
                actor.BroadcastMessage("DoAction", "NeutralAction");
            else
                actor.BroadcastMessage("DoAction", "Fall");
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
            GameObject.Destroy(entry.Value.gameObject);
        }
    }

    public override void stateTransitions()
    {
        base.stateTransitions();
        if (current_frame >= last_frame)
            if (actor.GetAbstractFighter().grounded)
                actor.BroadcastMessage("DoAction", "NeutralAction");
            else
                actor.BroadcastMessage("DoAction", "Fall");
        if (actor.GetAbstractFighter().grounded)
            actor.BroadcastMessage("DoAction", "Land");
    }
}