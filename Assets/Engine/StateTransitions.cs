using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StateTransitions : ScriptableObject {

    public static void NeutralState(AbstractFighter actor)
    {
        //shield
        if (actor.KeyBuffered(InputType.Attack))
            actor.doGroundAttack();
        //special
        if (actor.KeyBuffered(InputType.Jump))
            actor.doAction("Jump");
        if (actor.KeyBuffered(InputType.Down,threshold: 0.5f))
            actor.doAction("Crouch");
        if (actor.KeyHeld(InputTypeUtil.GetForward(actor)))
            actor.doAction("Move");
        if (actor.KeyHeld(InputTypeUtil.GetBackward(actor)))
        {
            actor.flip(); //TODO PIVOT
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
        if (actor.KeyBuffered(InputType.Shield))
        {
            //TODO forward backward roll
        }
        if (actor.KeyBuffered(InputType.Attack))
            actor.doAction("DownAttack");
        //if (actor.GetControllerButton("Special"))
        //doAction("DownSpecial")
        if (actor.KeyBuffered(InputType.Jump))
            actor.doAction("Jump");
        if (actor.KeyBuffered(InputType.Down,threshold:-0.1f) && actor._current_action.GetType() != typeof(CrouchGetup))
            actor.doAction("CrouchGetup");
    }

    public static void AirState(AbstractFighter actor)
    {
        StateTransitions.AirControl(actor);
        if (actor.GetControllerButton("Shield") && actor.air_dodges >= 1)
        {
            //actor.doAction("AirDodge");
        }
        if (actor.KeyBuffered(InputType.Attack))
        {
            actor.doAirAttack();
        }
        if (actor.GetControllerButton("Special"))
        {
            //specials TODO
        }
        if (actor.KeyBuffered(InputType.Jump) && actor.jumps > 0)
        {
            actor.doAction("AirJump");
        }
        //TODO fastfal
    }

    public static void MoveState(AbstractFighter actor)
    {
        List<KeyValuePair<InputType, float>> sequence = new List<KeyValuePair<InputType, float>>()
        {
            new KeyValuePair<InputType, float>(InputTypeUtil.GetForward(actor),0.0f),
            new KeyValuePair<InputType, float>(InputTypeUtil.GetForward(actor),0.5f)
        };
        if (actor.SequenceBuffered(sequence))
            Debug.Log("Running!");
        //float direction = actor.GetControllerAxis("Horizontal") * actor.facing;
        //shield
        if (actor.GetControllerButtonDown("Attack"))
            actor.doGroundAttack();
        //special
        if (actor.GetControllerButtonDown("Jump"))
            actor.doAction("Jump");
        else if (actor.GetControllerAxis("Vertical") < -0.5f)
            actor.doAction("Crouch");
        else if (actor.GetControllerAxis("Horizontal") == 0.0f)
            actor.doAction("Stop");
        if (actor.KeyBuffered(InputTypeUtil.GetBackward(actor)))
            actor.flip(); //TODO PIVOT
        //Two other kinds of stop? Not sure if these are needed
    }

    public static void StopState(AbstractFighter actor)
    {
        //shield
        //attack
        //special
        if (actor.GetControllerButtonDown("Jump"))
            actor.doAction("Jump");
        //if repeated, dash
        //pivot
    }
    
    public static void RunStopState(AbstractFighter actor)
    {

    }

    public static void DashState(AbstractFighter actor)
    {

    }

    public static void JumpState(AbstractFighter actor)
    {
        /*
        AirControl(actor);
        TapReversible(actor);
        if (actor.GetControllerButton("Shield") && actor.air_dodges >= 1)
            actor.doAction("AirDodge");
        //if (actor.GetControllerButton("Attack"))
        //actor.doAirAttack();
        //if (actor.GetControllerButton("Special"))
        //actor.doAirAttack();
        //Platform Phase and fastfall
        */
    }

    public static void ShieldState(AbstractFighter actor)
    {

    }

    public static void LedgeState(AbstractFighter actor)
    {

    }

    public static void GrabbingState(AbstractFighter actor)
    {

    }

    public static void ProneState(AbstractFighter actor)
    {

    }

    public static void AirControl(AbstractFighter actor)
    {
        actor._xPreferred = actor.GetControllerAxis("Horizontal") * actor.max_air_speed;
        if (Mathf.Abs(actor._xSpeed) > actor.max_air_speed)
            actor.accel(actor.air_control);
        if (Mathf.Abs(actor._ySpeed) > Mathf.Abs(actor.max_fall_speed))
            actor.landing_lag = actor.heavy_land_lag;
        if (actor.grounded && actor.ground_elasticity == 0 && actor.tech_window == 0)
        {
            actor._xPreferred = 0;
            actor._yPreferred = actor.max_fall_speed;
            actor.doAction("Land");
        }

    }
    
    public static void HelplessControl(AbstractFighter actor)
    {

    }

    public static void CheckLedges(AbstractFighter actor)
    {
        //TODO
    }

    public static void CheckGround(AbstractFighter actor)
    {
        if (!actor.grounded)
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
