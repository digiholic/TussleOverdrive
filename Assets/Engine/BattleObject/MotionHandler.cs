using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MotionHandler : BattleComponent {
    private CharacterController _charController;

    private float _xSpeed;
    public float XSpeed
    {
        get { return _xSpeed; }
        set { _xSpeed = value; }
    }

    private float _ySpeed;
    public float YSpeed
    {
        get { return _ySpeed; }
        set { _ySpeed = value; }
    }

    // Use this for initialization
    void Start () {
        _charController = GetComponent<CharacterController>();

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

        Vector3 movement = new Vector3(0, 0, 0);
        movement.y = _ySpeed;
        movement.x = _xSpeed;
        movement *= Time.deltaTime;
        _charController.Move(movement);
    }
}
