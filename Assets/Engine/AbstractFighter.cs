using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AbstractFighter : MonoBehaviour {
    public int player_num = 0;

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


    //Public variables that other classes need, but aren't set in the editor:
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
    [HideInInspector]
    public GameObject game_controller;
    [HideInInspector]
    public float damage_percent = 0;

    private CharacterController _charController;
    private SpriteRenderer sprite;
    private SpriteLoader sprite_loader;
    private actionLoader action_loader;
    private float last_x_axis;
    private float x_axis_delta;
    private float last_y_axis;
    private float y_axis_delta;


    void Start () {
        sprite = GetComponent<SpriteRenderer>();
        sprite_loader = GetComponent<SpriteLoader>();
        action_loader = GetComponent<actionLoader>();

        if (player_num % 2 == 0)
            facing = 1;
        else
            facing = -1;
        
        _ySpeed = 0;
        _charController = GetComponent<CharacterController>();
        jumps = max_jumps;
        _current_action = ScriptableObject.CreateInstance<NeutralAction>();
        _current_action.SetUp(this);
        game_controller = GameObject.Find("Controller");
    }

    // Update is called once per frame
    void Update () {
        if (_charController.isGrounded)
        {
            grounded = true;
            jumps = max_jumps;
        }
        else
        {
            grounded = false;
            _ySpeed += gravity * 5 * Time.deltaTime;
            if (_ySpeed < max_fall_speed || (_ySpeed < 0 && GetControllerAxis("Vertical") < -0.3))
            {
                _ySpeed = max_fall_speed;
            } 
        }
        float current_x = GetControllerAxis("Horizontal");
        x_axis_delta = Mathf.Abs(current_x) - Mathf.Abs(last_x_axis);
        last_x_axis = current_x;

        float current_y = GetControllerAxis("Vertical");
        y_axis_delta = Mathf.Abs(current_y) - Mathf.Abs(last_y_axis);
        last_y_axis = current_y;

        //if (x_axis_delta != 0 || y_axis_delta != 0)
        //    Debug.Log(x_axis_delta.ToString() + ',' + y_axis_delta.ToString());

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
        _current_action = action_loader.LoadAction(_actionName);
        old_action.TearDown(_current_action);
        Destroy(old_action);
        _current_action.SetUp(this);
    }

    /// <summary>
    /// Gets the horizontal direction relative to the direction of facing, so that positive is forward and negative is backward.
    /// </summary>
    /// <returns> The float value of the direction relative to facing</returns>
    public float GetDirectionRelative()
    {
        return GetControllerAxis("Horizontal") * facing;
    }

    void OnTriggerStay(Collider other)
    {
        if (other.tag == "DropDown" && GetControllerAxis("Vertical") < -0.3)
        {
            Physics.IgnoreCollision(_charController, other.transform.parent.GetComponent<Collider>(), true);
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.tag == "DropDown")
        {
            Physics.IgnoreCollision(_charController, other.transform.parent.GetComponent<Collider>(), false);
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
        //    sprite.flipX = !sprite.flipX;
        //else
        if (sprite != null) //Sprites get flipped
            transform.localScale = new Vector3(-transform.localScale.x, transform.localScale.y,transform.localScale.z);
        else //Models get rotated
            transform.Rotate(transform.rotation.x, 180, transform.rotation.z);
        facing *= -1;

    }

    public void doGroundAttack()
    {
        if (GetDirectionRelative() > 0.0f)
            doAction("ForwardAttack");
        else if (GetDirectionRelative() < 0.0f)
        {
            flip();
            doAction("ForwardAttack");
        }
        else if (GetControllerAxis("Vertical") > 0.0f)
            doAction("UpAttack");
        else if (GetControllerAxis("Vertical") < 0.0f)
            doAction("DownAttack");
        else
            doAction("NeutralAttack");
    }

    public void doAirAttack()
    {
        if (GetDirectionRelative() > 0.0f)
            doAction("ForwardAir");
        else if (GetDirectionRelative() < 0.0f)
            doAction("BackAir");
        else if (GetControllerAxis("Vertical") > 0.0f)
            doAction("UpAir");
        else if (GetControllerAxis("Vertical") < 0.0f)
            doAction("DownAir");
        else
            doAction("NeutralAir");
    }
    public void ChangeSprite(string sprite_name, int frame=0)
    {

        if (sprite_loader != null)
            sprite_loader.ChangeSprite(sprite_name, frame);
    }

    public void ChangeSubimage(int frame, bool loop=true)
    {
        if (sprite_loader != null)
            sprite_loader.ChangeSubimage(frame,loop);
    }


    public void GetHit(Hitbox hitbox)
    {
        Debug.Log("GetHit");

        float weight_constant = 1.4f;
        float flat_constant = 5.0f;

        float percent_portion = (damage_percent / 10.0f) + ((damage_percent * hitbox.damage) / 20.0f);
        float weight_portion = 200.0f / (weight * hitbox.weight_influence + 100);
        float scaled_kb = (((percent_portion * weight_portion * weight_constant) + flat_constant) * hitbox.knockback_growth);
        ApplyKnockback(scaled_kb + hitbox.base_knockback, hitbox.trajectory);
        DealDamage(hitbox.damage);
    }

    public void DealDamage(float _damage)
    {
        damage_percent += _damage;
        damage_percent = Mathf.Min(999, damage_percent);

        //TODO log damage data
    }

    public void ApplyHitstop(float _damage, float _hitlagMultiplier)
    {

    }

    public void ApplyKnockback(float _total_kb, float _trajectory)
    {
        //Debug.Log(_total_kb);
        _trajectory *= Mathf.Deg2Rad;
        Vector2 trajectory_vec = new Vector2(Mathf.Cos(_trajectory), Mathf.Sin(_trajectory));

        //DI
        //Vector2 trajectory_vec = new Vector2(Mathf.Cos(_trajectory / 180 * Mathf.PI), Mathf.Sin(_trajectory / 180 * Mathf.PI));
        //di_vec = self.getSmoothedInput(int(self.key_bindings.timing_window['smoothing_window']))
        //di_multiplier = 1 + numpy.dot(di_vec, trajectory_vec) * .05
        //_trajectory += numpy.cross(di_vec, trajectory_vec) * 13.5

        trajectory_vec *= _total_kb;
        //Debug.Log(trajectory_vec);
        _xSpeed = trajectory_vec.x;
        _ySpeed = trajectory_vec.y;
        //self.setSpeed((_total_kb) * di_multiplier, _trajectory)
    }

    public void ApplyHitstun(float _total_kb, float _hitstunMultiplier, float _baseHitstun, float _trajectory)
    {

    }
    /**
     * Shorthand for getting the input axis that this fighter is reading from.
     **/
    public float GetControllerAxis(string axisName)
    {
        return Input.GetAxisRaw(player_num + "_" + axisName);
    }

    public bool GetControllerButton(string buttonName)
    {
        return Input.GetButton(player_num + "_" + buttonName);
    }

    public bool GetControllerButtonDown(string buttonName)
    {
        return Input.GetButtonDown(player_num + "_" + buttonName);
    }

    public bool GetControllerButtonUp(string buttonName)
    {
        return Input.GetButtonUp(player_num + "_" + buttonName);
    }

    public bool CheckSmash(string axisName)
    {
        if (axisName == "Horizontal")
            return (x_axis_delta > 0.3);
        else
            return (y_axis_delta > 0.3);
    }
}
