using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class MotionHandler : BattleComponent {
    private CharacterController _charController;

    public float XSpeed { get; private set; }
    public float YSpeed { get; private set; }

    public float XPreferred { get; private set; }
    public float YPreferred { get; private set; }

    // Use this for initialization
    void Start () {
        _charController = GetComponent<CharacterController>();
        if (_charController == null)
        {
            _charController = gameObject.AddComponent<CharacterController>();
        }
    }

    // Update is called once per frame
    void Update () {
        if (_charController.isGrounded)
        {
            BroadcastMessage("SetGrounded", true);
        }
        else
        {
            BroadcastMessage("SetGrounded", false);
        }
    }

    void LateUpdate()
    {
        Vector3 movement = new Vector3(0, 0, 0);
        movement.y = YSpeed;
        movement.x = XSpeed;
        movement *= Time.deltaTime;
        _charController.Move(movement);
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
    /// The Single-arguemtn version of CalcGrav, for use with SendMessage
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
}
