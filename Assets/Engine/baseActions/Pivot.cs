using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pivot : GameAction {

    public Pivot()
    {
        SetVar("direction", 0);
    }
    public override void SetUp(BattleObject obj)
    {
        base.SetUp(obj);
        SetVar("direction", actor.GetIntVar("facing"));
        int num_frames = Mathf.FloorToInt((actor.GetMotionHandler().XSpeed * actor.GetIntVar("facing")) / actor.GetFloatVar("pivot_grip"));
        SetVar("num_frames", num_frames);
        //If the pivot grip would have us with a shorter amount than the length shows it should be
        //then we need to start partway through the pivot
        if (num_frames < last_frame)
            current_frame = Mathf.Min(last_frame - num_frames, last_frame - 1);
        else
            last_frame = num_frames;
    }

    public override void TearDown(GameAction new_action)
    {
        base.TearDown(new_action);
        if (new_action.HasVar("direction"))
            PassVariable("direction", GetIntVar("direction") * actor.GetIntVar("facing"));
        else if (actor.GetIntVar("facing") != GetIntVar("direction"))
            actor.BroadcastMessage("flip");
    }

    public override void stateTransitions()
    {
        base.stateTransitions();
        StateTransitions.StopState(actor.GetAbstractFighter());
        StateTransitions.CheckGround(actor.GetAbstractFighter());

        //(key, invkey) = _actor.getForwardBackwardKeys()
        //if (self.direction == 1 and _actor.sprite.flip == "left") or(self.direction == -1 and _actor.sprite.flip == "right"):
        //    _actor.sprite.flipX()

        AbstractFighter fighter = actor.GetAbstractFighter();
        if (current_frame == last_frame)
        {
            if (fighter.KeyHeld(InputTypeUtil.GetForward(actor)))
            {
                if (fighter.CheckSmash(InputTypeUtil.GetForward(actor)))
                    actor.SendMessage("doAction", "Dash");
                else
                    actor.SendMessage("doAction", "Move");

            }
            else if (fighter.KeyHeld(InputTypeUtil.GetBackward(actor)))
            {
                actor.SendMessage("flip");
                if (fighter.CheckSmash(InputTypeUtil.GetBackward(actor)))
                    actor.SendMessage("doAction", "Dash");
                else
                    actor.SendMessage("doAction", "Move");
            }
            else
                actor.SendMessage("doAction", "NeutralAction");
        }
    }

    public override void Update()
    {
        base.Update();
        if (current_frame == 0)
            actor.SendMessage("flip");
        //if _actor.keysContain(key) and _actor.keysContain(invkey):
        //_actor.preferred_xspeed = _actor.stats['max_ground_speed'] * _actor.facing
        //_actor.accel(_actor.stats['static_grip'])
        //else:
        actor.SendMessage("ChangeXPreferred", 0.0f);
        actor.GetMotionHandler().accel(actor.GetFloatVar("pivot_grip"));
    }
}
