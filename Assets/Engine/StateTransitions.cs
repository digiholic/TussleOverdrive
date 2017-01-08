using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StateTransitions : ScriptableObject {

    public static void NeutralState(AbstractFighter actor)
    {
        //shield
        if (actor.GetControllerButtonDown("Attack"))
            actor.doGroundAttack();
        //special
        if (actor.GetControllerButtonDown("Jump"))
            actor.doAction("Jump");
        if (actor.GetControllerAxis("Vertical") < -0.5f)
            actor.doAction("Crouch");
        if (actor.GetControllerAxis("Horizontal") != 0.0f)
        {
            float direction = actor.GetControllerAxis("Horizontal");
            if (direction * actor.facing < 0.0f) //If the movement and facing do not match
                actor.flip();
            actor.doAction("Move");
        }
    }

    public static void CrouchState(AbstractFighter actor)
    {
        if (actor.GetControllerButton("Shield"))
        {
            //TODO forward backward roll
        }
        //if (actor.GetControllerButton("Attack"))
        //doAction("DownTilt")
        //if (actor.GetControllerButton("Special"))
        //doAction("DownSpecial")
        if (actor.GetControllerButton("Jump"))
            actor.doAction("Jump");
        if (actor.GetControllerAxis("Vertical") >= -0.5f && actor._current_action.GetType() != typeof(CrouchGetup))
            actor.doAction("CrouchGetup");
    }

    public static void AirState(AbstractFighter actor)
    {
        StateTransitions.AirControl(actor);
        if (actor.GetControllerButton("Shield") && actor.air_dodges >= 1)
        {
            //actor.doAction("AirDodge");
        }
        if (actor.GetControllerButton("Attack"))
        {
            //attacks TODO
        }
        if (actor.GetControllerButton("Special"))
        {
            //specials TODO
        }
        if (actor.GetControllerButtonDown("Jump") && actor.jumps > 0)
        {
            actor.doAction("AirJump");
        }
        //TODO fastfal
    }

    public static void MoveState(AbstractFighter actor)
    {
        float direction = actor.GetControllerAxis("Horizontal") * actor.facing;
        //shield
        //attack
        //special
        if (actor.GetControllerButtonDown("Jump"))
            actor.doAction("Jump");
        else if (actor.GetControllerAxis("Vertical") < -0.5f)
            actor.doAction("Crouch");
        else if (actor.GetControllerAxis("Horizontal") == 0.0f)
            actor.doAction("Stop");
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

    public static void GrabLedges(AbstractFighter actor)
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
}
