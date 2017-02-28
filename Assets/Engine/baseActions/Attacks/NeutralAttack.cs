using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NeutralAttack : BaseAttack {

    public override void SetUp(AbstractFighter _actor)
    {
        length = 22;
        sprite_name = "neutral";
        sprite_rate = 1;
        base.SetUp(_actor);
        //Debug.Log("NeutralAttack Created");

        Dictionary<string, float> dict = new Dictionary<string, float>()
        {
            {"damage", 2 },
            {"base_knockback", 10 },
            {"knockback_growth", 0.1f },
            {"trajectory", 90 }
        };

        Hitbox hbox = game_controller.GetComponent<HitboxLoader>().LoadHitbox(actor, this, dict, 0,0,92,92);
        hitboxes.Add("hitbox", hbox);
    }

    public override void Update()
    {
        base.Update();
        if (current_frame == 4)
        {
            hitboxes["hitbox"].Activate(5);
        }
    }
}
