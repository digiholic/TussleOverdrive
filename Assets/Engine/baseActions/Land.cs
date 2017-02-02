using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Land : GameAction {

    public override void SetUp(AbstractFighter _actor)
    {
        length = 6;
        sprite_name = "land";
        sprite_rate = 2;
        loop = false;
        base.SetUp(_actor);
        //Debug.Log("LandAction created");
        //Set speed to platform speed
        /*block = reduce(lambda x, y: y if x is None or y.rect.top <= x.rect.top else x, _actor.checkGround(), None)
        if not block is None:
            _actor.change_y = block.change_y
            _actor.posy = block.rect.top - _actor.ecb.previous_ecb.rect.height/2.0 */
    }

    public override void stateTransitions()
    {
        base.stateTransitions();
        if (current_frame >= last_frame)
        {
            actor.landing_lag = 0;
            actor.doAction("NeutralAction");
            //platform phase reset
            actor._xPreferred = 0;
        }
        StateTransitions.CheckGround(actor);
    }

    public override void Update()
    {
        base.Update();
        if (current_frame == 0)
        {
            actor._yPreferred = actor.max_fall_speed;
            last_frame = Mathf.Max(last_frame,actor.landing_lag);
            actor._ySpeed = -1.0f;
            //L Cancel
            /* lcancel = settingsManager.getSetting('lagCancel')
            if lcancel == 'normal':
                if _actor.keyHeld('shield', 4) and not _actor.keyBuffered('shield', 20, 0.1, 4):
                    print("l-cancel")
                    self.last_frame = self.last_frame // 2
            elif lcancel == 'auto':
                print("l-cancel")
                self.last_frame = self.last_frame // 2 */
        }
    }
}
