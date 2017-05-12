using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LedgeGrab : GameAction {
    private Ledge grabbed_ledge;

    public override void SetUp(BattleObject obj)
    {
        base.SetUp(obj);
        grabbed_ledge = actor.GetAbstractFighter().GrabbedLedge;

    }
    public override void stateTransitions()
    {
        base.stateTransitions();
        StateTransitions.LedgeState(actor.GetAbstractFighter());
    }

    public override void TearDown(GameAction new_action)
    {
        base.TearDown(new_action);
        actor.SendMessage("ReleaseLedge");
    }

    public override void Update()
    {
        base.Update();
        actor.SendMessage("Rest");

        int facingDir = actor.GetAbstractFighter().facing;

        if (grabbed_ledge.grabSide == Ledge.Side.LEFT)
            if (facingDir == -1)
                actor.GetAbstractFighter().flip();
        if (grabbed_ledge.grabSide == Ledge.Side.RIGHT)
            if (facingDir == 1)
                actor.GetAbstractFighter().flip();
        //Snap to point

        actor.transform.FindChild("Hang_Point").SendMessage("SnapAnchorToPoint", grabbed_ledge.transform.GetChild(0).position);
        actor.transform.FindChild("Hang_Point").SendMessage("MoveAnchor", new Vector2(0.0f, 0.1f));
        //snapPoint.x = (grabbed_ledge.transform.localScale.x*actor.GetAbstractFighter().facing) + (0.5f * (actor.transform.localScale.x) * actor.GetAbstractFighter().facing);
        //snapPoint.x = -0.5f*(actor.transform.localScale.x)*actor.GetAbstractFighter().facing;

        actor.SendMessage("ChangeXSpeed",0f);
        actor.SendMessage("ChangeYSpeed", 0f);
    }
}