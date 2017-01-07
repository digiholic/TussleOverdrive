using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NeutralAttack : Action {

    public override void SetUp(AbstractFighter _actor)
    {
        length = 22;
        sprite_name = "neutral";
        sprite_rate = 1;
        base.SetUp(_actor);
        Debug.Log("NeutralAttack Created");
    }

    public override void Update()
    {
        base.Update();
        if (current_frame == 4)
        {
            actor.activateHitbox();
        }
    }

    public override void stateTransitions()
    {
        base.stateTransitions();
        if (current_frame > last_frame)
            actor.doAction("NeutralAction");
    }
}
