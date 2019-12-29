using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
[RequireComponent (typeof(BattleObjectCollider2D))]
public class MotionHandler : BattleComponent {
    //private CharacterController _charController;
    //private Rigidbody rigid;
    private BattleObjectCollider2D controller;

    public float max_fall_speed;
    
    [SerializeField] private Vector3 movement;
    void Awake()
    {
        controller = GetComponent<BattleObjectCollider2D>();
    }
    // Use this for initialization
    void Start () {
        SetVar(TussleConstants.MotionVariableNames.XSPEED, 0f);
        SetVar(TussleConstants.MotionVariableNames.YSPEED, 0f);
        SetVar(TussleConstants.MotionVariableNames.XPREF, 0f);
        SetVar(TussleConstants.MotionVariableNames.YPREF, 0f);
    }

    // Update is called once per frame
    public override void ManualUpdate () {
        //if (_charController.isGrounded) SetVar(TussleConstants.FighterVariableNames.IS_GROUNDED, true);
        //else SetVar(TussleConstants.FighterVariableNames.IS_GROUNDED,false);
    }

    public void ExecuteMovement()
    {
        movement.x = GetFloatVar(TussleConstants.MotionVariableNames.XSPEED);
        movement.y = GetFloatVar(TussleConstants.MotionVariableNames.YSPEED);
        
        movement *= Time.deltaTime;
        controller.Move(movement);
        //transform.Translate(movement);
        //rigid.velocity = transform.TransformDirection(movement);

    }

    void Update()
    {
        Debug.DrawRay(transform.position, movement);

    }

    /// <summary>
    /// Change the X Speed to the given value.
    /// </summary>
    /// <param name="_xSpeed">The speed to set X to</param>
    public void ChangeXSpeed(float _xSpeed)
    {
        SetVar(TussleConstants.MotionVariableNames.XSPEED, _xSpeed);
    }

    /// <summary>
    /// Change the X Speed by a set amount. Adds the given value to the current XSpeed.
    /// </summary>
    /// <param name="_xSpeed">The value to change X Speed By</param>
    public void ChangeXSpeedBy(float _xSpeed)
    {
        float xSpeed = GetFloatVar(TussleConstants.MotionVariableNames.XSPEED);
        SetVar(TussleConstants.MotionVariableNames.XSPEED, xSpeed + _xSpeed);
    }

    /// <summary>
    /// Change the Y Speed to the given value.
    /// </summary>
    /// <param name="_ySpeed">The speed to set Y to</param>
    public void ChangeYSpeed(float _ySpeed)
    {
        SetVar(TussleConstants.MotionVariableNames.YSPEED, _ySpeed);
    }

    /// <summary>
    /// Change the Y Speed by a set amount. Adds the given value to the current YSpeed.
    /// </summary>
    /// <param name="_ySpeed">The value to change Y Speed By</param>
    public void ChangeYSpeedBy(float _ySpeed)
    {
        float ySpeed = GetFloatVar(TussleConstants.MotionVariableNames.YSPEED);
        SetVar(TussleConstants.MotionVariableNames.YSPEED, ySpeed + _ySpeed);
    }

    /// <summary>
    /// Change the X and Y speeds by providing a Vector
    /// </summary>
    /// <param name="_newSpeed">A Vector3 representing the new speed to change to. Note that z is ignored, but Vector3 must be used anyway for Unity purposes</param>
    public void ChangeSpeedVector(Vector3 _newSpeed)
    {
        ChangeXSpeed(_newSpeed.x);
        ChangeYSpeed(_newSpeed.y);
    }

    /// <summary>
    /// Add the values of a vector to the X and Y Speed
    /// </summary>
    /// <param name="_newSpeed">A Vector3 representing the new speed to add. Note that z is ignored, but Vector3 must be used anyway for Unity purposes</param>

    public void ChangeSpeedVectorBy(Vector3 _additionalSpeed)
    {
        ChangeXSpeedBy(_additionalSpeed.x);
        ChangeYSpeedBy(_additionalSpeed.y);
    }

    /// <summary>
    /// Change the X Preferred to the given value.
    /// </summary>
    /// <param name="_xPreferred">The Preferred to set X to</param>
    public void ChangeXPreferred(float _xPreferred)
    {
        SetVar(TussleConstants.MotionVariableNames.XPREF, _xPreferred);
    }

    /// <summary>
    /// Change the X Preferred by a set amount. Adds the given value to the current XPreferred.
    /// </summary>
    /// <param name="_xPreferred">The value to change X Preferred By</param>
    public void ChangeXPreferredBy(float _xPreferred)
    {
        float xpref = GetFloatVar(TussleConstants.MotionVariableNames.XPREF);
        SetVar(TussleConstants.MotionVariableNames.XSPEED, xpref + _xPreferred);
    }

    /// <summary>
    /// Change the Y Preferred to the given value.
    /// </summary>
    /// <param name="_yPreferred">The Preferred to set Y to</param>
    public void ChangeYPreferred(float _yPreferred)
    {
        SetVar(TussleConstants.MotionVariableNames.YPREF, _yPreferred);
    }

    /// <summary>
    /// Change the Y Preferred by a set amount. Adds the given value to the current YPreferred.
    /// </summary>
    /// <param name="_yPreferred">The value to change Y Preferred By</param>
    public void ChangeYPreferredBy(float _yPreferred)
    {
        float ypref = GetFloatVar(TussleConstants.MotionVariableNames.YPREF);
        SetVar(TussleConstants.MotionVariableNames.XSPEED, ypref + _yPreferred);
    }

    /// <summary>
    /// Change the X and Y preferred speeds by providing a Vector
    /// </summary>
    /// <param name="_newSpeed">A Vector3 representing the new speed to change to. Note that z is ignored, but Vector3 must be used anyway for Unity purposes</param>
    public void ChangePreferredVector(Vector3 _newSpeed)
    {
        ChangeXPreferred(_newSpeed.x);
        ChangeYPreferred(_newSpeed.y);
    }

    /// <summary>
    /// Add the values of a vector to the X and Y Preferred Speed
    /// </summary>
    /// <param name="_newSpeed">A Vector3 representing the new speed to add. Note that z is ignored, but Vector3 must be used anyway for Unity purposes</param>

    public void ChangePreferredVectorBy(Vector3 _additionalSpeed)
    {
        ChangeXPreferredBy(_additionalSpeed.x);
        ChangeYPreferredBy(_additionalSpeed.y);
    }

    public void accel(float _xFactor)
    {
        float XSpeed = GetFloatVar(TussleConstants.MotionVariableNames.XSPEED);
        float XPreferred = GetFloatVar(TussleConstants.MotionVariableNames.XPREF);

        if (XSpeed > XPreferred)
        {
            float diff = XSpeed - XPreferred;
            XSpeed -= Mathf.Min(diff, _xFactor);
        }
        else if (XSpeed < XPreferred)
        {
            float diff = XPreferred - XSpeed;
            XSpeed += Mathf.Min(diff, _xFactor);
        }
        ChangeXSpeed(XSpeed);
    }

    /// <summary>
    /// The Single-argument version of CalcGrav, for use with SendMessage
    /// </summary>
    /// <param name="args">A list containing the gravity and the max_fall_speed, in that order</param>
    public void CalcGrav(float[] args)
    {
        CalcGrav(args[0], args[1]);
    }

    public void CalcGrav(float gravity, float max_fall_speed)
    {
        //FIXME gravity calculations are hella floaty. Need to rework gravity entirely
        ChangeYSpeedBy(gravity * Settings.current_settings.gravity_ratio * 5 * Time.deltaTime);
        if (GetFloatVar(TussleConstants.MotionVariableNames.YSPEED) < max_fall_speed)
        {
            ChangeYSpeed(max_fall_speed);
        }
    }

    public static Vector2 GetDirectionMagnitude(BattleObject actor)
    {
        return GetDirectionMagnitude(actor.GetFloatVar(TussleConstants.MotionVariableNames.XSPEED), actor.GetFloatVar(TussleConstants.MotionVariableNames.YSPEED));
    }

    public static Vector2 GetDirectionMagnitude(float XSpeed,float YSpeed)
    {
        float magnitude;
        float direction;
        if (XSpeed == 0) //Shortcut if xspeed is zero
        {
            magnitude = YSpeed;
            if (YSpeed < 0)
                direction = 270;
            else
                direction = 90;
        }
        if (YSpeed == 0) //Shortcut if yspeed is zero
        {
            magnitude = XSpeed;
            if (XSpeed < 0)
                direction = 180;
            else
                direction = 0;
        }

        direction = Mathf.Atan2(YSpeed, XSpeed) * Mathf.Rad2Deg;
        direction = Mathf.Round(direction);
        magnitude = new Vector2(XSpeed, YSpeed).magnitude;

        Vector2 retVec = new Vector2(direction, magnitude);
        return retVec;
    }

    public Vector3 GetMotionVector()
    {
        float XSpeed = GetFloatVar(TussleConstants.MotionVariableNames.XSPEED);
        float YSpeed = GetFloatVar(TussleConstants.MotionVariableNames.YSPEED);

        return new Vector3(XSpeed, YSpeed, 0.0f);
    }
}
