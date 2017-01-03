using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StateTransitions : ScriptableObject {

    public static void NeutralState(AbstractFighter actor)
    {
        //shield
        //attack
        //special
        if (Input.GetButtonDown("Jump"))
            actor.doAction("Jump");
        if (Input.GetAxis("Vertical") < -0.5f)
            actor.doAction("Crouch");
        if (Input.GetAxis("Horizontal") != 0.0f)
        {
            float direction = Input.GetAxis("Horizontal");
            if (direction * actor.facing < 0.0f) //If the movement and facing do not match
                actor.flip();
            actor.doAction("Move");
        }
    }

    public static void AirState(AbstractFighter actor)
    {
        StateTransitions.AirControl(actor);
        if (actor.grounded)
        {
            actor._ySpeed = -1.0f;
            actor.doAction("NeutralAction");
        }
        if (Input.GetButtonDown("Jump") && actor.jumps > 0)
        {
            actor.doAction("AirJump");
        }
    }

    public static void AirControl(AbstractFighter actor)
    {
        actor._xPreferred = Input.GetAxis("Horizontal") * actor.max_air_speed;
        if (Mathf.Abs(actor._xSpeed) > actor.max_air_speed)
            actor.accel(actor.air_control);
        //if (Mathf.Abs(actor._ySpeed) > Mathf.Abs(actor.max_fall_speed))
        //actor.landing_lag = actor.heavy_land_lag;
        /*
        if _actor.grounded and _actor.ground_elasticity == 0 and _actor.tech_window == 0:
        _actor.preferred_xspeed = 0
        _actor.preferred_yspeed = _actor.stats['max_fall_speed']
        _actor.doAction('Land')
        */

    }

    public static void CheckGround(AbstractFighter actor)
    {
        if (!actor.grounded)
        {
            actor.doAction("Fall");
        }
    }

    public static void GrabLedges(AbstractFighter actor)
    {
        //TODO
    }

    public static void JumpState(AbstractFighter actor)
    {
        //TODO
    }

    public static void CrouchState(AbstractFighter actor)
    {
        if (Input.GetButton("Shield"))
        {
            //TODO forward backward roll
        }
        //if (Input.GetButton("Attack"))
        //doAction("DownTilt")
        //if (Input.GetButton("Special"))
        //doAction("DownSpecial")
        if (Input.GetButton("Jump"))
            actor.doAction("Jump");
        if (Input.GetAxis("Vertical") >= -0.5f && actor._current_action.GetType() != typeof(CrouchGetup))
            actor.doAction("CrouchGetup");
    }
    
    public static void MoveState(AbstractFighter actor)
    {
        float direction = Input.GetAxis("Horizontal") * actor.facing;
        //shield
        //attack
        //special
        if (Input.GetButtonDown("Jump"))
            actor.doAction("Jump");
        else if (Input.GetAxis("Vertical") < -0.5f)
            actor.doAction("Crouch");
        else if (Input.GetAxis("Horizontal") == 0.0f)
        {
            actor.doAction("NeutralAction"); //actor.doAction("Stop");
            Debug.Log("Stopping");
        }
            
        //Two other kinds of stop? Not sure if these are needed
    }
}
