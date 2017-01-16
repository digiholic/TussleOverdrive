using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dair : AirAttack {

    public override void SetUp(AbstractFighter _actor)
    {
        length = 39;
        sprite_name = "dair";
        sprite_rate = 3;
        base.SetUp(_actor);
    }

    public override void Update()
    {
        base.Update();
    }
}
