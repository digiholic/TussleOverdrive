using System.Collections;
using System.Collections.Generic;
using System.Xml;
using System.IO;
using UnityEngine;
using UnityEngine.Profiling;

public class AbstractFighter : MonoBehaviour {
    public string fighter_xml_file = "";
    public int player_num = 0;

    [HideInInspector]
    public string fighter_name = "Unknown", franchise_icon = "Assets/Sprites/Defaults/franchise_icon.png", css_icon = "Assets/Sprites/Defaults/css_icon.png", css_portrait = "Assets/Sprites/Default/css_portrait.png";
    [HideInInspector]
    public string sprite_directory = "./sprites/", sprite_prefix = "", default_sprite = "idle",pixels_per_unit = "100";
    [HideInInspector]
    public string article_path = "", article_file = "", sound_path = "";
    [HideInInspector]
    public string action_file = "";
    
    [HideInInspector]
    public float weight = 10.0f, gravity = -9.8f, max_fall_speed = -20.0f, max_ground_speed = 7.0f, run_speed = 11.0f, max_air_speed = 5.5f, crawl_speed = 2.5f, dodge_sepeed = 8.5f, friction = 0.3f, static_grip = 0.3f, pivot_grip = 0.6f, air_resistance = 0.2f, air_control = 0.2f, jump_height = 15.0f, short_hop_height = 8.5f, air_jump_height = 18.0f, fastfall_multiplier = 2.0f, hitstun_elasticity = 0.8f, shield_size = 1.0f, aerial_transition_speed = 9.0f;
    /* Editor visible version
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
    public float jump_height = 15.0f;
    public float short_hop_height = 8.5f;
    public float air_jump_height = 18.0f;
    public float fastfall_multiplier = 2.0f;
    public float hitstun_elasticity = 0.8f;
    public float shield_size = 1.0f;
    */
    [HideInInspector]
    public int max_jumps = 1, heavy_land_lag = 4, wavedash_lag = 12;

    //Public variables that other classes need, but aren't set in the editor:
    [HideInInspector]
    public bool grounded = false;

    [HideInInspector]
    public int jumps = 0, facing = 1, landing_lag = 0, tech_window = 0, air_dodges = 1;

    [HideInInspector]
    public float _xSpeed, _ySpeed, _xPreferred, _yPreferred, ground_elasticity = 0.0f, damage_percent = 0;

    [HideInInspector]
    public GameAction _current_action;

    [HideInInspector]
    public GameController game_controller;
    
    private CharacterController _charController;
    private SpriteRenderer sprite;
    private SpriteLoader sprite_loader;
    private actionLoader action_loader;
    private Animator anim;
    private float last_x_axis;
    private float x_axis_delta;
    private float last_y_axis;
    private float y_axis_delta;
    private InputBuffer inputBuffer;
    private XMLLoader data_xml;

    private ActionFile actions_file_json = new ActionFile();
    private DynamicAction current_dynamic_action;

    void Awake()
    {
        if (File.Exists(fighter_xml_file))
        {
            data_xml = GetComponent<XMLLoader>();
            data_xml.LoadXML(fighter_xml_file);
            
            fighter_name = data_xml.SelectSingleNode("//fighter/name").GetString();
            franchise_icon = data_xml.SelectSingleNode("//fighter/icon").GetString();
            css_icon = data_xml.SelectSingleNode("//fighter/css_icon").GetString();
            css_portrait = data_xml.SelectSingleNode("//fighter/css_portrait").GetString();

            sprite_directory = data_xml.SelectSingleNode("//fighter/sprite_directory").GetString();
            sprite_prefix = data_xml.SelectSingleNode("//fighter/sprite_prefix").GetString();
            default_sprite = data_xml.SelectSingleNode("//fighter/default_sprite").GetString();
            pixels_per_unit = data_xml.SelectSingleNode("//fighter/pixels_per_unit").GetString();

            article_path = data_xml.SelectSingleNode("//fighter/article_path").GetString();
            article_file = data_xml.SelectSingleNode("//fighter/articles").GetString();
            sound_path = data_xml.SelectSingleNode("//fighter/sound_path").GetString();

            action_file = data_xml.SelectSingleNode("//fighter/actions").GetString();

            //Load the stats
            weight = GetFromXml("weight", weight);
            gravity = GetFromXml("gravity", gravity);
            max_fall_speed = GetFromXml("max_fall_speed", max_fall_speed);
            max_ground_speed = GetFromXml("max_ground_speed", max_ground_speed);
            run_speed = GetFromXml("run_speed", run_speed);
            max_air_speed = GetFromXml("max_air_speed", max_air_speed);
            aerial_transition_speed = GetFromXml("aerial_transition_speed", aerial_transition_speed);
            crawl_speed = GetFromXml("crawl_speed", crawl_speed);
            dodge_sepeed = GetFromXml("dodge_speed", dodge_sepeed);
            friction = GetFromXml("friction", friction);
            static_grip = GetFromXml("static_grip", static_grip);
            pivot_grip = GetFromXml("pivot_grip", pivot_grip);
            air_resistance = GetFromXml("air_resistance", air_resistance);
            air_control = GetFromXml("air_control", air_control);
            jump_height = GetFromXml("jump_height", jump_height);
            short_hop_height = GetFromXml("short_hop_height", short_hop_height);
            air_jump_height = GetFromXml("air_jump_height", air_jump_height);
            fastfall_multiplier = GetFromXml("fastfall_multiplier", fastfall_multiplier);
            hitstun_elasticity = GetFromXml("hitstun_elasticity", hitstun_elasticity);
            shield_size = GetFromXml("shield_size", shield_size);
            max_jumps = Mathf.FloorToInt(GetFromXml("max_jumps", max_jumps));
            heavy_land_lag = Mathf.FloorToInt(GetFromXml("heavy_land_lag", heavy_land_lag));
            wavedash_lag = Mathf.FloorToInt(GetFromXml("wavedash_lag", wavedash_lag));

            string action_json_path = Path.Combine(data_xml.root_directory.FullName, action_file);
            if (File.Exists(action_json_path))
            {
                string action_json = File.ReadAllText(action_json_path);
                actions_file_json = JsonUtility.FromJson<ActionFile>(action_json);
            }
        }
        else
        {
            Debug.LogWarning("Could not find Fighter XML for player " + player_num);
        }

    }

    void Start() {
        sprite = GetComponent<SpriteRenderer>();
        sprite_loader = GetComponent<SpriteLoader>();
        action_loader = GetComponent<actionLoader>();
        anim = GetComponent<Animator>();
        inputBuffer = GetComponent<InputBuffer>();
        
        if (player_num % 2 == 0)
            facing = 1;
        else
            facing = -1;

        _ySpeed = 0;
        _charController = GetComponent<CharacterController>();
        jumps = max_jumps;
        actions_file_json.BuildDict();
        _current_action = ScriptableObject.CreateInstance<NeutralAction>();
        _current_action.SetUp(this);
        current_dynamic_action = actions_file_json.Get("NeutralAction");
        current_dynamic_action.ExecuteGroup("SetUp",this,_current_action);
        game_controller = GameObject.Find("Controller").GetComponent<GameController>();

    }

    private float GetFromXml(string stat_name, float default_value)
    {
        DataNode data_node = data_xml.SelectSingleNode("//fighter/stats/" + stat_name);
        if (data_node != null)
            return data_node.GetFloat();
        else
            return default_value;
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
        current_dynamic_action.ExecuteGroup("StateTransitions",this,_current_action);
        current_dynamic_action.ExecuteGroup("BeforeFrame", this, _current_action);
        _current_action.Update();
        current_dynamic_action.ExecuteFrame(this, _current_action);
        _current_action.LateUpdate();
        current_dynamic_action.ExecuteGroup("AfterFrame", this, _current_action);
        
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
        //Debug.Log("GameAction: "+_actionName);
        GameAction old_action = _current_action;
        _current_action = action_loader.LoadAction(_actionName);
        current_dynamic_action.ExecuteGroup("TearDown", this, old_action);
        old_action.TearDown(_current_action);
        Destroy(old_action);
        current_dynamic_action = actions_file_json.Get(_actionName);
        _current_action.SetUp(this);
        current_dynamic_action.ExecuteGroup("SetUp", this, _current_action);
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
        if (anim != null)
        {
            if (anim.HasState(0,Animator.StringToHash(sprite_name)))
                anim.CrossFade(sprite_name, 0f);
        }
            
    }

    public void ChangeSubimage(int frame, bool loop=true)
    {
        if (sprite_loader != null)
            sprite_loader.ChangeSubimage(frame,loop);
    }


    public void GetHit(Hitbox hitbox)
    {
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

    public bool KeyBuffered(InputType key, int distance = 12, float threshold = 0.1f)
    {
        return inputBuffer.KeyBuffered(key, distance, threshold);
    }

    public bool KeyHeld(InputType key)
    {
        return inputBuffer.ControllerState[key] > 0.0f;
    }

    public bool SequenceBuffered(List<KeyValuePair<InputType,float>> inputList, int distance = 12)
    {
        return inputBuffer.SequenceBuffered(inputList, distance);
    }


    public bool CheckSmash(string axisName)
    {
        if (axisName == "Horizontal")
            return (x_axis_delta > 0.3);
        else
            return (y_axis_delta > 0.3);
    }

    public void TestActionJSON()
    {
        string action_json_path = Path.Combine(data_xml.root_directory.FullName, action_file);
        if (File.Exists(action_json_path))
        {
            string action_json = File.ReadAllText(action_json_path);
            actions_file_json = JsonUtility.FromJson<ActionFile>(action_json);
        }
        /*
        ActionGroup testSetUp = new ActionGroup();
        testSetUp.subactions = new List<string>() { "Test String" };
        DynamicAction testAction = new DynamicAction("Test Action 2");
        testAction.tear_down_actions = testSetUp;
        testAction.actions_at_frame.Add(testSetUp);
        actions_file_json.Add(testAction);
        */
        File.WriteAllText(action_json_path, JsonUtility.ToJson(actions_file_json, true));
    }
}