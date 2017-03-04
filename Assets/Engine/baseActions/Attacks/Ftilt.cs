using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ftilt : BaseAttack {

    public override void SetUp(AbstractFighter _actor)
    {
        length = 24;
        sprite_name = "fsmash";
        sprite_rate = 2;
        base.SetUp(_actor);
        //Debug.Log("NeutralAttack Created");

        Dictionary<string, string> dict = new Dictionary<string, string>()
        {
            {"damage", "10" },
            {"base_knockback", "1.5" },
            {"knockback_growth", "0.2" },
            {"trajectory", "50" }
        };

        Hitbox hbox = game_controller.GetComponent<HitboxLoader>().LoadHitbox(actor, this, dict);
        hitboxes.Add("hitbox", hbox);
    }

    public override void Update()
    {
        base.Update();
        if (current_frame == 14)
        {
            hitboxes["hitbox"].Activate(2);
        }
    }
}
