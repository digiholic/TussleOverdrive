using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AbstractFighter : MonoBehaviour {

    public int max_jumps = 1;

    public float weight = 10.0f;
    public float jumpSpeed = 15.0f;
    public float gravity = -9.8f;
    public float max_fall_speed = -10.0f;
    public float min_fall_speed = -1.5f;

    private int _jumps;
    private float _ySpeed;
    private CharacterController _charController;
    // Use this for initialization
    void Start () {
        _ySpeed = min_fall_speed;
        _charController = GetComponent<CharacterController>();
        _jumps = max_jumps;
	}
	
	// Update is called once per frame
	void Update () {
        if (_charController.isGrounded)
        {
            _jumps = max_jumps;
            {
                _ySpeed = min_fall_speed;
            }
        }
        else
        {
            _ySpeed += gravity * 5 * Time.deltaTime;
            if (_ySpeed < max_fall_speed || (_ySpeed < 0 && Input.GetAxis("Vertical") < -0.3))
            {
                _ySpeed = max_fall_speed;
            } 
            
        }

        //Handle jumps
        if (Input.GetButtonDown("Jump"))
        {
            if (_charController.isGrounded || _jumps > 0)
            {
                _ySpeed = jumpSpeed;
            }
            if (!_charController.isGrounded)
            {
                _jumps -= 1;
            }
        }

        Vector3 movement = new Vector3(0, 0, 0);
        movement.y = _ySpeed;
        movement *= Time.deltaTime;
        _charController.Move(movement);
    }
}
