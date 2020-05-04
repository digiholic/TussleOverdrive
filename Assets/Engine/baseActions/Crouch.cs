using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Crouch : GameAction {

    public override void stateTransitions()
    {
        base.stateTransitions();
        //These classes will be phased out as time goes on. Until then, we need to just exit early if we're in the builder since these don't actually use Subactions
        if (isInBuilder) return;
        StateTransitions.CrouchState(actor.GetAbstractFighter());
        StateTransitions.CheckGround(actor.GetAbstractFighter());
    }

    public override void TearDown(GameAction new_action)
    {
        base.TearDown(new_action);
        //These classes will be phased out as time goes on. Until then, we need to just exit early if we're in the builder since these don't actually use Subactions
        if (isInBuilder) return;
        actor.BroadcastMessage("ChangeXPreferred", 0.0f, SendMessageOptions.RequireReceiver);
        //Remove crouch armor
    }

    public override void Update()
    {
        base.Update();
        //These classes will be phased out as time goes on. Until then, we need to just exit early if we're in the builder since these don't actually use Subactions
        if (isInBuilder) return;
        //TODO crawl action
        //actor.BroadcastMessage("ChangeXPreferred", actor.GetAbstractFighter().GetControllerAxis("Horizontal") * actor.GetFloatVar(TussleConstants.FighterAttributes.CRAWL_SPEED));

        //actor.GetComponent<SpriteLoader>().printSprite();
        if (current_frame >= last_frame)
            current_frame = last_frame - 1;
    }
}
