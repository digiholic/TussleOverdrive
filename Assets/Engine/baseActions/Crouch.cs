using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Crouch : GameAction {

    public override void stateTransitions()
    {
        base.stateTransitions();
        StateTransitions.CrouchState(actor);
        StateTransitions.CheckGround(actor);
        //Platform Phase? See if it's better as it's own thing or better off here.
        /* Python code:
         * if self.frame > 0 and _actor.keyBuffered('down', _state = 1):
            blocks = _actor.checkGround()
            if blocks:
                #Turn it into a list of true/false if the block is solid
                blocks = map(lambda x:x.solid,blocks)
                #If none of the ground is solid
                if not any(blocks):
                    _actor.doAction('PlatformDrop')
         */
    }

    public override void TearDown(GameAction new_action)
    {
        base.TearDown(new_action);
        actor._xPreferred = 0;
        //Remove crouch armor
    }

    public override void Update()
    {
        base.Update();
        actor.accel(actor.pivot_grip);
        actor._xPreferred = actor.GetControllerAxis("Horizontal") * actor.crawl_speed;
        //actor.GetComponent<SpriteLoader>().printSprite();
    }

    public override void LateUpdate()
    {
        base.LateUpdate();
        if (current_frame >= last_frame)
            current_frame = last_frame-1;
    }
}
