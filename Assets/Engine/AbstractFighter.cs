using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AbstractFighter : MonoBehaviour {
    private CharacterController _charController;

    public float weight = 10.0f;
    public float gravity = -9.8f;
    public float max_fall_speed = -20.0f;
    public float max_ground_speed = 7.0f;
    public float run_speed = 11.0f;
    public float max_air_speed = 5.5f;
    public float crawl_speed = 2.5f;
    public float dodge_sepeed = 8.5f;
    public float friction = 0.3f;
    public float static_grip = 0.3f;
    public float pivot_grip = 0.6f;
    public float air_resistance = 0.2f;
    public float air_control = 0.2f;
    public int max_jumps = 1;
    public float jump_height = 15.0f;
    public float short_hop_height = 8.5f;
    public float air_jump_height = 18.0f;
    public int heavy_land_lag = 4;
    public int wavedash_lag = 12;
    public float fastfall_multiplier = 2.0f;
    public float hitstun_elasticity = 0.8f;
    public float shield_size = 1.0f;
    
    public float aerial_transition_speed = 9.0f;


    //Public varialbes that other classes need, but aren't set in the editor:
    [HideInInspector]
    public bool grounded = false;
    [HideInInspector]
    public int jumps = 0;
    [HideInInspector]
    public float _xSpeed;
    [HideInInspector]
    public float _ySpeed;
    [HideInInspector]
    public float _xPreferred;
    [HideInInspector]
    public float _yPreferred;
    [HideInInspector]
    public Action _current_action;
    [HideInInspector]
    public int facing = 1;
    [HideInInspector]
    public int landing_lag = 0;
    [HideInInspector]
    public float ground_elasticity = 0.0f;
    [HideInInspector]
    public int tech_window = 0;
    [HideInInspector]
    public int air_dodges = 1;
    // Use this for initialization
    void Start () {
        _ySpeed = 0;
        _charController = GetComponent<CharacterController>();
        jumps = max_jumps;
        _current_action = ScriptableObject.CreateInstance<NeutralAction>();
        _current_action.SetUp(this);
	}
	
	// Update is called once per frame
	void Update () {
        if (_charController.isGrounded)
        {
            grounded = true;
            jumps = max_jumps;
            _ySpeed = -1.0f;
        }
        else
        {
            grounded = false;
            _ySpeed += gravity * 5 * Time.deltaTime;
            if (_ySpeed < max_fall_speed || (_ySpeed < 0 && Input.GetAxis("Vertical") < -0.3))
            {
                _ySpeed = max_fall_speed;
            } 
        }

        _current_action.stateTransitions();
        _current_action.Update();
        _current_action.LateUpdate();


        if (grounded)
            accel(friction);
        else
            accel(air_resistance);

        Vector3 movement = new Vector3(0, 0, 0);
        movement.y = _ySpeed;
        movement.x = _xSpeed;
        movement *= Time.deltaTime;
        _charController.Move(movement);
    }
    
    public void doAction(string _actionName)
    {
        //Debug.Log("Action: "+_actionName);
        Action old_action = _current_action;
        _current_action = GetComponent<actionLoader>().LoadAction(_actionName);
        old_action.TearDown(_current_action);
        Destroy(old_action);
        _current_action.SetUp(this);
    }

    void OnTriggerStay(Collider other)
    {
        if (other.tag == "DropDown" && Input.GetAxis("Vertical") < -0.3)
        {
            Physics.IgnoreCollision(GetComponent<CharacterController>(), other.transform.parent.GetComponent<Collider>(), true);
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.tag == "DropDown")
        {
            Physics.IgnoreCollision(GetComponent<CharacterController>(), other.transform.parent.GetComponent<Collider>(), false);
        }
    }

    public void accel(float _xFactor)
    {
        //TODO global friction/airControl values
        float accel_fric = 1.0f; //friction
        if (!grounded)
            accel_fric = 1.0f; //air control

        if (_xSpeed > _xPreferred)
        {
            float diff = _xSpeed - _xPreferred;
            _xSpeed -= Mathf.Min(diff, _xFactor * accel_fric);
        } else if (_xSpeed < _xPreferred)
        {
            float diff = _xPreferred - _xSpeed;
            _xSpeed += Mathf.Min(diff, _xFactor * accel_fric);
        }
    }

    public void flip()
    {
        SpriteRenderer SpriteRender = GetComponent<SpriteRenderer>();
        SpriteRender.flipX = !SpriteRender.flipX;
        facing *= -1;
    }
}
