using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Crouch : GameAction {

    public override void stateTransitions()
    {
        base.stateTransitions();
        StateTransitions.CrouchState(actor.GetAbstractFighter());
        StateTransitions.CheckGround(actor.GetAbstractFighter());
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
        actor.BroadcastMessage("ChangeXPreferred", 0.0f);
        //Remove crouch armor
    }

    public override void Update()
    {
        base.Update();
        actor.GetMotionHandler().accel(actor.GetFloatVar("pivot_grip"));
        //TODO crawl action
        //actor.BroadcastMessage("ChangeXPreferred", actor.GetAbstractFighter().GetControllerAxis("Horizontal") * actor.GetFloatVar("crawl_speed"));

        //actor.GetComponent<SpriteLoader>().printSprite();
        if (current_frame >= last_frame)
            current_frame = last_frame - 1;
    }
}
