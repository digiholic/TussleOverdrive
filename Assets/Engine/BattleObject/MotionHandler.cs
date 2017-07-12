using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class MotionHandler : BattleComponent {
    private CharacterController _charController;
    //private Rigidbody rigid;

    public float XSpeed { get; private set; }
    public float YSpeed { get; private set; }

    public float XPreferred { get; private set; }
    public float YPreferred { get; private set; }


    public float gravity;
    public float max_fall_speed;
    
    // Use this for initialization
    void Start () {
        _charController = GetComponent<CharacterController>();
        //rigid = GetComponent<Rigidbody>();
        if (_charController == null)
        {
            _charController = gameObject.AddComponent<CharacterController>();
        }
    }

    // Update is called once per frame
    public override void ManualUpdate () {
        //if (_charController.isGrounded) SetVar("grounded", true);
        //else SetVar("grounded",false);
    }

    public void ExecuteMovement()
    {
        Vector3 movement = new Vector3(0, 0, 0);
        movement.y = YSpeed;
        movement.x = XSpeed;

        movement *= Time.deltaTime;
        _charController.Move(movement);
        
        //transform.Translate(movement);
        //rigid.velocity = transform.TransformDirection(movement);

    }

    void Update()
    {
        Vector3 movement = new Vector3(0, 0, 0);
        movement.y = YSpeed;
        movement.x = XSpeed;
        movement *= Time.deltaTime;
        Debug.DrawRay(transform.position, movement * 10);

    }

    /// <summary>
    /// Change the X Speed to the given value.
    /// </summary>
    /// <param name="_xSpeed">The speed to set X to</param>
    public void ChangeXSpeed(float _xSpeed)
    {
        XSpeed = _xSpeed;
    }

    /// <summary>
    /// Change the X Speed by a set amount. Adds the given value to the current XSpeed.
    /// </summary>
    /// <param name="_xSpeed">The value to change X Speed By</param>
    public void ChangeXSpeedBy(float _xSpeed)
    {
        XSpeed += _xSpeed;
    }

    /// <summary>
    /// Change the Y Speed to the given value.
    /// </summary>
    /// <param name="_ySpeed">The speed to set Y to</param>
    public void ChangeYSpeed(float _ySpeed)
    {
        YSpeed = _ySpeed;
    }

    /// <summary>
    /// Change the Y Speed by a set amount. Adds the given value to the current YSpeed.
    /// </summary>
    /// <param name="_ySpeed">The value to change Y Speed By</param>
    public void ChangeYSpeedBy(float _ySpeed)
    {
        YSpeed += _ySpeed;
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
        XPreferred = _xPreferred;
    }

    /// <summary>
    /// Change the X Preferred by a set amount. Adds the given value to the current XPreferred.
    /// </summary>
    /// <param name="_xPreferred">The value to change X Preferred By</param>
    public void ChangeXPreferredBy(float _xPreferred)
    {
        XPreferred += _xPreferred;
    }

    /// <summary>
    /// Change the Y Preferred to the given value.
    /// </summary>
    /// <param name="_yPreferred">The Preferred to set Y to</param>
    public void ChangeYPreferred(float _yPreferred)
    {
        YPreferred = _yPreferred;
    }

    /// <summary>
    /// Change the Y Preferred by a set amount. Adds the given value to the current YPreferred.
    /// </summary>
    /// <param name="_yPreferred">The value to change Y Preferred By</param>
    public void ChangeYPreferredBy(float _yPreferred)
    {
        YPreferred += _yPreferred;
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
        ChangeYSpeedBy(gravity * 5 * Time.deltaTime);
        if (YSpeed < max_fall_speed)
        {
            ChangeYSpeed(max_fall_speed);
        }
    }

    public Vector2 GetDirectionMagnitude()
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
        direction = Mathf.Atan2(YSpeed,XSpeed) * Mathf.Rad2Deg;
        direction = Mathf.Round(direction);
        magnitude = new Vector2(XSpeed, YSpeed).magnitude;

        Vector2 retVec = new Vector2(direction, magnitude);
        return retVec;
    }
    
    public Vector3 GetMotionVector()
    {
        return new Vector3(XSpeed, YSpeed, 0.0f);
    }

    void OnControllerColliderHit(ControllerColliderHit hit)
    {
        if (hit.gameObject.layer == LayerMask.NameToLayer("Terrain"))
        {
            
        }
        
    }
}
