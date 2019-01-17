using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LedgeGrab : GameAction {
    private Ledge grabbed_ledge;

    public override void SetUp(BattleObject obj)
    {
        base.SetUp(obj);
        //These classes will be phased out as time goes on. Until then, we need to just exit early if we're in the builder since these don't actually use Subactions
        if (isInBuilder) return;
        grabbed_ledge = actor.GetAbstractFighter().GrabbedLedge;
    }
    public override void stateTransitions()
    {
        base.stateTransitions();
        //These classes will be phased out as time goes on. Until then, we need to just exit early if we're in the builder since these don't actually use Subactions
        if (isInBuilder) return;
        StateTransitions.LedgeState(actor.GetAbstractFighter());
    }

    public override void TearDown(GameAction new_action)
    {
        base.TearDown(new_action);
        //These classes will be phased out as time goes on. Until then, we need to just exit early if we're in the builder since these don't actually use Subactions
        if (isInBuilder) return;
        actor.SendMessage("ReleaseLedge");
    }

    public override void Update()
    {
        base.Update();
        //These classes will be phased out as time goes on. Until then, we need to just exit early if we're in the builder since these don't actually use Subactions
        if (isInBuilder) return;
        actor.SendMessage("Rest");
        if (grabbed_ledge != null)
        {

            actor.GetAnchorPoint("Hang_Point").SendMessage("SnapAnchorToPoint", grabbed_ledge.hang_point);

            actor.SendMessage("ChangeXSpeed", 0f);
            actor.SendMessage("ChangeYSpeed", 0f);

            int facingDir = actor.GetIntVar(TussleConstants.FighterVariableNames.FACING_DIRECTION);

            if (grabbed_ledge.grabSide == Ledge.Side.LEFT)
                if (facingDir == -1)
                    actor.SendMessage("flip");
            if (grabbed_ledge.grabSide == Ledge.Side.RIGHT)
                if (facingDir == 1)
                    actor.SendMessage("flip");
        }
    }
}