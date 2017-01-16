using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bair : AirAttack {

    public override void SetUp(AbstractFighter _actor)
    {
        length = 35;
        sprite_name = "bair";
        sprite_rate = 0;
        base.SetUp(_actor);
    }

    public override void Update()
    {
        base.Update();
        if (current_frame == 1)
            actor.ChangeSubimage(1, loop);
        if (current_frame == 2)
            actor.ChangeSubimage(2, loop);
        if (current_frame == 4)
            actor.ChangeSubimage(3, loop);
        if (current_frame == 6)
            actor.ChangeSubimage(4, loop);
        if (current_frame == 8)
            actor.ChangeSubimage(5, loop);
        if (current_frame == 12)
            actor.ChangeSubimage(6, loop);
        if (current_frame == 16)
            actor.ChangeSubimage(7, loop);
        if (current_frame == 20)
            actor.ChangeSprite("jump");
    }
}
