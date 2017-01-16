using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Utilt : BaseAttack {

    public override void SetUp(AbstractFighter _actor)
    {
        length = 28;
        sprite_name = "utilt";
        sprite_rate = 4;
        base.SetUp(_actor);
        //Debug.Log("NeutralAttack Created");

        Dictionary<string, float> dict = new Dictionary<string, float>()
        {
            {"damage", 7 },
            {"base_knockback", 8 },
            {"knockback_growth", 0.08f },
            {"trajectory", 100 }
        };

        Hitbox hbox = game_controller.GetComponent<HitboxLoader>().LoadHitbox(actor, dict, 0, 0, 92, 92);
        hitboxes.Add("hitbox", hbox);
    }

    public override void Update()
    {
        base.Update();
        if (current_frame == 4)
        {
            hitboxes["hitbox"].Activate(4);
        }
    }
}
