using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Uair : AirAttack {

    public override void SetUp(AbstractFighter _actor)
    {
        length = 34;
        sprite_name = "uair";
        sprite_rate = 3;
        base.SetUp(_actor);
    }

    public override void Update()
    {
        base.Update();
        if (current_frame == 9)
        {
            sprite_rate = 0;
            actor.ChangeSubimage(2);
        }
        if (current_frame == 18)
            actor.ChangeSubimage(3);
        if (current_frame == 21)
            actor.ChangeSubimage(4);
    }
}
