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

        Dictionary<string, float> sweetdict = new Dictionary<string, float>()
        {
            {"damage", 7 },
            {"base_knockback", 8 },
            {"knockback_growth", 0.08f },
            {"trajectory", 100 }
        };

        Hitbox sweet = game_controller.GetComponent<HitboxLoader>().LoadHitbox(actor, this, sweetdict, 30, 20, 6, 20);
        hitboxes.Add("sweet", sweet);

        Dictionary<string, float> tangydict = new Dictionary<string, float>()
        {
            {"damage", 5 },
            {"base_knockback", 7 },
            {"knockback_growth", 0.08f },
            {"trajectory", 110 }
        };

        Hitbox tangy = game_controller.GetComponent<HitboxLoader>().LoadHitbox(actor, this, tangydict, 27, 25, 12, 35);
        hitboxes.Add("tangy", tangy);

        Dictionary<string, float> sourdict = new Dictionary<string, float>()
        {
            {"damage", 3 },
            {"base_knockback", 6 },
            {"knockback_growth", 0.08f },
            {"trajectory", 120 }
        };

        Hitbox sour = game_controller.GetComponent<HitboxLoader>().LoadHitbox(actor, this, sourdict, 21, 15, 24, 60);
        hitboxes.Add("sour", sour);
    }

    public override void Update()
    {
        base.Update();
        if (current_frame == 4)
            hitboxes["sweet"].Activate(4);
        if (current_frame == 8)
            hitboxes["tangy"].Activate(4);
        if (current_frame == 12)
            hitboxes["sour"].Activate(6);
    }
}
