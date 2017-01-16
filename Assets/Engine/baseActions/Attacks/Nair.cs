using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Nair : AirAttack {

    public override void SetUp(AbstractFighter _actor)
    {
        length = 40;
        sprite_name = "nair";
        loop = true;
        base.SetUp(_actor);
    }

    public override void Update()
    {
        base.Update();
    }
}
