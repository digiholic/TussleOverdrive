using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fair : AirAttack {

    public override void SetUp(AbstractFighter _actor)
    {
        length = 44;
        sprite_name = "fair";
        sprite_rate = 2;
        base.SetUp(_actor);
    }

    public override void Update()
    {
        base.Update();
        if (current_frame == 26)
            actor.ChangeSprite("jump");
    }
}
