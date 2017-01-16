using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dtilt : BaseAttack {

    public override void SetUp(AbstractFighter _actor)
    {
        length = 32;
        sprite_name = "dtilt";
        sprite_rate = 2;
        loop = true;
        base.SetUp(_actor);
        //Debug.Log("NeutralAttack Created");
    }

    public override void Update()
    {
        base.Update();
    }
}
