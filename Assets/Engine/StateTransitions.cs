﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StateTransitions : ScriptableObject {
    public static void NeutralState(AbstractFighter actor)
    {
        //shield
        if (actor.KeyBuffered("Attack"))
            actor.doGroundAttack();
        if (actor.KeyBuffered("Special"))
            actor.doGroundSpecial();
        if (actor.KeyBuffered("Jump"))
            actor.doAction("Jump");
        //if (actor.KeyBuffered("Shield"))
        //    actor.doAction("Shield");
        // if (actor.DirectionHeld("Down"))
        //     actor.doAction("Crouch");
        if (actor.DirectionHeld("Forward"))
            actor.doAction("Move");
        if (actor.DirectionHeld("Backward"))
        {
            actor.SendMessage("flip"); //TODO PIVOT
            actor.doAction("Move");
        }

        /*
        if (actor.GetControllerAxis("Horizontal") != 0.0f)
        {
            float direction = actor.GetControllerAxis("Horizontal");
            if (direction * actor.facing < 0.0f) //If the movement and facing do not match
                actor.flip();
            actor.doAction("Move");
        }
        */
    }

    public static void CrouchState(AbstractFighter actor)
    {
        //The getup state has everything we need, except for actually calling "getup" so we use that, and then add our extra bit at the end
        CrouchGetupState(actor);
        if (!actor.DirectionHeld("Down"))
            actor.doAction("CrouchGetup");
    }

    public static void CrouchGetupState(AbstractFighter actor)
    {
        if (actor.KeyBuffered("Shield"))
        {
            //TODO forward backward roll
        }
        if (actor.KeyBuffered("Attack"))
        {
            if (actor.CheckSmash("DownSmash")) actor.doAction("DownSmash");
            else actor.doAction("DownAttack");
        }

        if (actor.KeyBuffered("Special"))
            actor.doAction("DownSpecial");
        if (actor.KeyBuffered("Jump"))
            actor.doAction("Jump");
    }

    public static void AirState(AbstractFighter actor)
    {
        StateTransitions.AirControl(actor);
        if (actor.KeyBuffered("Shield") && actor.GetIntVar(TussleConstants.FighterVariableNames.AIR_DODGES_REMAINING) >= 1)
        {
            //actor.doAction("AirDodge");
        }
        if (actor.KeyBuffered("Attack"))
        {
            actor.doAirAttack();
        }
        if (actor.KeyBuffered("Special"))
        {
            actor.doAirSpecial();
        }
        if (actor.KeyBuffered("Jump") && actor.GetIntVar(TussleConstants.FighterVariableNames.JUMPS_REMAINING) > 0)
        {
            actor.doAction("AirJump");
        }
        actor.SendMessage("CheckForGround");
        if (actor.GetBoolVar(TussleConstants.FighterVariableNames.IS_GROUNDED) && actor.ground_elasticity == 0 && actor.GetIntVar("tech_window") == 0)
        {
            actor.BroadcastMessage("ChangeXPreferred", 0.0f, SendMessageOptions.RequireReceiver);
            actor.BroadcastMessage("ChangeYPreferred", actor.GetFloatVar(TussleConstants.FighterAttributes.MAX_FALL_SPEED));
            actor.doAction("Land");
        }
        //TODO fastfal
    }

    public static void MoveState(AbstractFighter actor)
    {
        if (actor.CheckSmash("ForwardSmash"))
            actor.doAction("Dash");
        //float direction = actor.GetControllerAxis("Horizontal") * actor.facing;
        //shield
        if (actor.KeyBuffered("Attack"))
            actor.doGroundAttack();
        if (actor.KeyBuffered("Special"))
            actor.doGroundSpecial();
        if (actor.KeyBuffered("Jump"))
            actor.doAction("Jump");
        // else if (actor.DirectionHeld("Down"))
        //     actor.doAction("Crouch");
        else if (actor.GetAxis("Horizontal") == 0.0f)
            actor.doAction("Stop");
        if (actor.KeyBuffered(InputTypeUtil.GetBackward(actor.getBattleObject())))
            actor.SendMessage("flip"); //TODO PIVOT
        //Two other kinds of stop? Not sure if these are needed
    }

    public static void StopState(AbstractFighter actor)
    {
        //shield
        //attack
        //special
        if (actor.KeyBuffered("Jump"))
            actor.doAction("Jump");
        //if repeated, dash
        //pivot
    }
    
    public static void RunStopState(AbstractFighter actor)
    {

    }

    public static void DashState(AbstractFighter actor)
    {
        /*
         * (key,invkey) = _actor.getForwardBackwardKeys()
            if _actor.keyHeld('attack'):
                if _actor.keysContain('shield'):
                    _actor.doAction('DashGrab')
                elif _actor.checkSmash(key):
                    print("Dash cancelled into forward smash")
                    _actor.doAction('ForwardSmash')
                else:
                    _actor.doAction('DashAttack')
            elif _actor.keyHeld('special'):
                _actor.doGroundSpecial()
            elif _actor.keysContain('down', 0.5):
                _actor.doAction('RunStop')
            elif not _actor.keysContain('left') and not _actor.keysContain('right') and not _actor.keysContain('down'):
                _actor.doAction('RunStop')
            elif _actor.preferred_xspeed < 0 and not _actor.keysContain('left',1) and _actor.keysContain('right',1):
                _actor.doAction('RunStop')
            elif _actor.preferred_xspeed > 0 and not _actor.keysContain('right',1) and _actor.keysContain('left',1):
                _actor.doAction('RunStop')
        */
        //float direction = actor.GetControllerAxis("Horizontal") * actor.facing;
        //shield
        if (actor.KeyBuffered("Attack"))
            actor.doGroundAttack();
        if (actor.KeyBuffered("Special"))
            actor.doGroundSpecial();
        if (actor.KeyBuffered("Jump"))
            actor.doAction("Jump");
        else if (actor.GetAxis("Horizontal") == 0.0f)
            actor.doAction("Stop");
        if (actor.KeyBuffered(InputTypeUtil.GetBackward(actor.getBattleObject())))
            actor.SendMessage("flip"); //TODO PIVOT
    }

    public static void JumpState(AbstractFighter actor)
    {
        //AirControl(actor);
        TapReversible(actor);
        //if (actor.GetControllerButton("Shield") && actor.air_dodges >= 1)
        //    actor.doAction("AirDodge");
        //if (actor.KeyBuffered("Attack"))
        //    actor.doAirAttack();
        //if (actor.GetControllerButton("Special"))
        //actor.doAirAttack();
        //Platform Phase and fastfall
    }

    public static void ShieldState(AbstractFighter actor)
    {

    }

    public static void LedgeState(AbstractFighter actor)
    {
        /*
         * (key,invkey) = _actor.getForwardBackwardKeys()
    _actor.setSpeed(0, _actor.getFacingDirection())
    if _actor.keyHeld('shield'):
        _actor.ledge_lock = True
        _actor.doAction('LedgeRoll')
    elif _actor.keyHeld('attack'):
        _actor.ledge_lock = True
        _actor.doAction('LedgeAttack')
    elif _actor.keyHeld('jump'):
        _actor.ledge_lock = True
        apply_invuln = statusEffect.TemporaryHitFilter(_actor,hurtbox.Intangibility(_actor),6)
        apply_invuln.activate()
        _actor.doAction('Jump')
    elif _actor.keyHeld(key):
        _actor.ledge_lock = True
        _actor.doAction('LedgeGetup')
    elif _actor.keyHeld(invkey):
        _actor.ledge_lock = True
        apply_invuln = statusEffect.TemporaryHitFilter(_actor,hurtbox.Intangibility(_actor),6)
        apply_invuln.activate()
        _actor.doAction('Fall')
    elif _actor.keyHeld('down'):
        _actor.ledge_lock = True
        apply_invuln = statusEffect.TemporaryHitFilter(_actor,hurtbox.Intangibility(_actor),6)
        apply_invuln.activate()
        _actor.doAction('Fall')
        */
        if (actor.KeyBuffered("Jump"))
        {
            actor.doAction("Jump");
        }
        else if (actor.DirectionHeld("Backward"))
        {
            actor.doAction("Fall");
        }
    }

    public static void GrabbingState(AbstractFighter actor)
    {

    }

    public static void ProneState(AbstractFighter actor)
    {

    }

    public static void AirControl(AbstractFighter actor)
    {
        actor.BroadcastMessage("ChangeXPreferred", actor.GetAxis("Horizontal") * actor.GetFloatVar(TussleConstants.FighterAttributes.MAX_AIR_SPEED));
        if (Mathf.Abs(actor.GetFloatVar(TussleConstants.MotionVariableNames.XSPEED)) > actor.GetFloatVar(TussleConstants.FighterAttributes.MAX_AIR_SPEED))
            actor.SendMessage("accel", actor.GetFloatVar(TussleConstants.FighterAttributes.AIR_CONTROL));
        if (Mathf.Abs(actor.GetFloatVar(TussleConstants.MotionVariableNames.YSPEED)) > Mathf.Abs(actor.GetFloatVar(TussleConstants.FighterAttributes.MAX_FALL_SPEED)))
            actor.SetVar("landing_lag", actor.GetIntVar(TussleConstants.FighterAttributes.HEAVY_LANDING_LAG));
    }
    
    public static void HelplessControl(AbstractFighter actor)
    {

    }

    public static void CheckLedges(AbstractFighter actor)
    {
        if (!actor.LedgeLock) //If the lock is active, no need to bother calculating anything
        {
            foreach (Ledge ledge in actor.GetLedges())
            {
                if (!actor.DirectionHeld("Down"))
                {
                    if ((ledge.grabSide == Ledge.Side.LEFT) && actor.DirectionHeld("Right"))
                        ledge.SendMessage("FighterGrabs", actor);
                    else if ((ledge.grabSide == Ledge.Side.RIGHT) && actor.DirectionHeld("Left"))
                        ledge.SendMessage("FighterGrabs", actor);
                }
            }
        }
    }


    public static void CheckGround(AbstractFighter actor)
    {
        actor.SendMessage("CheckForGround");
        if (!actor.GetBoolVar(TussleConstants.FighterVariableNames.IS_GROUNDED))
        {
            actor.doAction("Fall");
        }
    }

    public static void TiltReversible(AbstractFighter actor)
    {

    }

    public static void TapReversible(AbstractFighter actor)
    {

    }

    public static void ShieldCancellable(AbstractFighter actor)
    {

    }

    public static void DodgeCancellable(AbstractFighter actor)
    {

    }

    public static void AutoDodgeCancellable(AbstractFighter actor)
    {

    }

    public static void JumpCancellable(AbstractFighter actor)
    {

    }

    public static void LoadTransitionState(string state, AbstractFighter actor)
    {
        switch (state)
        {
            case "NeutralState":
                NeutralState(actor);
                break;
            case "CrouchState":
                CrouchState(actor);
                break;
            case "AirState":
                AirState(actor);
                break;
            case "MoveState":
                MoveState(actor);
                break;
            case "StopState":
                StopState(actor);
                break;
            case "RunStopState":
                RunStopState(actor);
                break;
            case "DashState":
                DashState(actor);
                break;
            case "JumpState":
                JumpState(actor);
                break;
            case "ShieldState":
                ShieldState(actor);
                break;
            case "LedgeState":
                LedgeState(actor);
                break;
            case "GrabbingState":
                GrabbingState(actor);
                break;
            case "ProneState":
                ProneState(actor);
                break;
            case "AirControl":
                AirControl(actor);
                break;
            case "HelplessControl":
                HelplessControl(actor);
                break;
            case "CheckLedges":
                CheckLedges(actor);
                break;
            case "CheckGround":
                CheckGround(actor);
                break;
            case "TiltReversible":
                TiltReversible(actor);
                break;
            case "TapReversible":
                TapReversible(actor);
                break;
            case "ShieldCancellable":
                ShieldCancellable(actor);
                break;
            case "DodgeCancellable":
                DodgeCancellable(actor);
                break;
            case "AutoDodgeCancellable":
                AutoDodgeCancellable(actor);
                break;
            case "JumpCancellable":
                JumpCancellable(actor);
                break;
        }
    }
}