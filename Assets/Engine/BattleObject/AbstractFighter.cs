using System.Collections;
using System.Collections.Generic;
using System.Xml;
using System.IO;
using UnityEngine;
using UnityEngine.Profiling;

public class AbstractFighter : MonoBehaviour {
    //public string fighter_xml_file = "";
    private string resource_path = "";
    public int player_num = 0;

    [HideInInspector]
    public string fighter_name = "Unknown", franchise_icon = "Assets/Sprites/Defaults/franchise_icon.png", css_icon = "Assets/Sprites/Defaults/css_icon.png", css_portrait = "Assets/Sprites/Default/css_portrait.png";
    [HideInInspector]
    public string sprite_directory = "./sprites/", sprite_prefix = "", default_sprite = "idle";
    [HideInInspector]
    public string article_path = "", article_file = "", sound_path = "";
    [HideInInspector]
    public string action_file = "";
    
    [HideInInspector]
    public float weight = 10.0f, gravity = -9.8f, max_fall_speed = -20.0f, max_ground_speed = 7.0f, run_speed = 11.0f, max_air_speed = 5.5f, crawl_speed = 2.5f, dodge_sepeed = 8.5f, friction = 0.3f, static_grip = 0.3f, pivot_grip = 0.6f, air_resistance = 0.2f, air_control = 0.2f, jump_height = 15.0f, short_hop_height = 8.5f, air_jump_height = 18.0f, fastfall_multiplier = 2.0f, hitstun_elasticity = 0.8f, shield_size = 1.0f, aerial_transition_speed = 9.0f, pixels_per_unit = 100;

    [HideInInspector]
    public int max_jumps = 1, heavy_land_lag = 4, wavedash_lag = 12;

    //Public variables that other classes need, but aren't set in the editor:
    [HideInInspector]
    public bool grounded = false;

    [HideInInspector]
    public int jumps = 0, facing = 1, landing_lag = 0, tech_window = 0, air_dodges = 1;

    [HideInInspector]
    public float ground_elasticity = 0.0f, damage_percent = 0;
    
    [HideInInspector]
    public BattleController game_controller;
    
    
    private SpriteRenderer sprite;
    private SpriteHandler sprite_loader;
    private Animator anim;
    private float last_x_axis;
    private float x_axis_delta;
    private float last_y_axis;
    private float y_axis_delta;
    private InputBuffer inputBuffer;
    private XMLLoader data_xml;

    private AudioSource sound_player;
    private List<HitboxLock> hitbox_locks = new List<HitboxLock>();
    private Dictionary<string, AudioClip> sounds = new Dictionary<string, AudioClip>();
    public BattleObject battleObject;

    private PlatformPhase platform_phaser;

    private MotionHandler motionHandler;

    void LoadFighterXML()
    {
        battleObject = GetComponent<BattleObject>();
        data_xml = GetComponent<XMLLoader>();
        resource_path = data_xml.resource_path;   

        if (data_xml != null)
        {
            fighter_name = data_xml.SelectSingleNode("//fighter/name").GetString();
            franchise_icon = data_xml.SelectSingleNode("//fighter/icon").GetString();
            css_icon = data_xml.SelectSingleNode("//fighter/css_icon").GetString();
            css_portrait = data_xml.SelectSingleNode("//fighter/css_portrait").GetString();

            sprite_directory = data_xml.SelectSingleNode("//fighter/sprite_directory").GetString();
            sprite_prefix = data_xml.SelectSingleNode("//fighter/sprite_prefix").GetString();
            default_sprite = data_xml.SelectSingleNode("//fighter/default_sprite").GetString();
            pixels_per_unit = float.Parse(data_xml.SelectSingleNode("//fighter/pixels_per_unit").GetString());

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

            /*
            battleObject.SetVar("fighter_name", data_xml.SelectSingleNode("//fighter/name").GetString());
            battleObject.SetVar("franchise_icon", data_xml.SelectSingleNode("//fighter/icon").GetString());
            battleObject.SetVar("css_icon", data_xml.SelectSingleNode("//fighter/css_icon").GetString());
            battleObject.SetVar("css_portrait", data_xml.SelectSingleNode("//fighter/css_portrait").GetString());

            article_path = data_xml.SelectSingleNode("//fighter/article_path").GetString();
            article_file = data_xml.SelectSingleNode("//fighter/articles").GetString();
            sound_path = data_xml.SelectSingleNode("//fighter/sound_path").GetString();

            action_file = data_xml.SelectSingleNode("//fighter/actions").GetString();

            //Load the stats
            battleObject.SetVar("weight", data_xml.SelectSingleNode("//fighter/stats/weight"));
            battleObject.SetVar("gravity", data_xml.SelectSingleNode("//fighter/stats/gravity"));
            battleObject.SetVar("max_fall_speed", data_xml.SelectSingleNode("//fighter/stats/max_fall_speed"));
            battleObject.SetVar("max_ground_speed", data_xml.SelectSingleNode("//fighter/stats/max_ground_speed"));
            battleObject.SetVar("run_speed", data_xml.SelectSingleNode("//fighter/stats/run_speed"));
            battleObject.SetVar("max_air_speed", data_xml.SelectSingleNode("//fighter/stats/max_air_speed"));
            battleObject.SetVar("aerial_transition_speed", data_xml.SelectSingleNode("//fighter/stats/aerial_transition_speed"));
            battleObject.SetVar("crawl_speed", data_xml.SelectSingleNode("//fighter/stats/crawl_speed"));
            battleObject.SetVar("dodge_speed", data_xml.SelectSingleNode("//fighter/stats/dodge_speed"));
            battleObject.SetVar("friction", data_xml.SelectSingleNode("//fighter/stats/friction"));
            battleObject.SetVar("static_grip", data_xml.SelectSingleNode("//fighter/stats/static_grip"));
            battleObject.SetVar("pivot_grip", data_xml.SelectSingleNode("//fighter/stats/pivot_grip"));
            battleObject.SetVar("air_resistance", data_xml.SelectSingleNode("//fighter/stats/air_resistance"));
            battleObject.SetVar("air_control", data_xml.SelectSingleNode("//fighter/stats/air_control"));
            battleObject.SetVar("jump_height", data_xml.SelectSingleNode("//fighter/stats/jump_height"));
            battleObject.SetVar("short_hop_height", data_xml.SelectSingleNode("//fighter/stats/short_hop_height"));
            battleObject.SetVar("air_jump_height", data_xml.SelectSingleNode("//fighter/stats/air_jump_height"));
            battleObject.SetVar("fastfall_multiplier", data_xml.SelectSingleNode("//fighter/stats/fastfall_multiplier"));

            battleObject.SetVar("hitstun_elasticity", data_xml.SelectSingleNode("//fighter/stats/hitstun_elasticity"));
            battleObject.SetVar("shield_size", data_xml.SelectSingleNode("//fighter/stats/shield_size"));
            battleObject.SetVar("max_jumps", data_xml.SelectSingleNode("//fighter/stats/max_jumps"));
            battleObject.SetVar("heavy_landing_lag", data_xml.SelectSingleNode("//fighter/stats/heavy_landing_lag"));
            battleObject.SetVar("wavedash_lag", data_xml.SelectSingleNode("//fighter/stats/wavedash_lag"));

            */


            string action_json_path = Path.Combine("Assets/Resources/" + resource_path, action_file);
            if (File.Exists(action_json_path))
            {
                string action_json = File.ReadAllText(action_json_path);
                battleObject.GetActionHandler().actions_file_json = JsonUtility.FromJson<ActionFile>(action_json);
                battleObject.GetActionHandler().actions_file_json.BuildDict();
            }
        }
        else
        {
            throw new System.Exception("Could not load FighterXML");
        }
    }

    void Start() {
        LoadFighterXML();
        sprite = GetComponent<SpriteRenderer>();
        sprite_loader = GetComponent<SpriteHandler>();
        sprite_loader.Initialize("Assets/Resources/" + resource_path + sprite_directory,sprite_prefix,pixels_per_unit);
        anim = GetComponent<Animator>();
        inputBuffer = GetComponent<InputBuffer>();
        sound_player = GetComponent<AudioSource>();
        platform_phaser = GetComponent<PlatformPhase>();
        motionHandler = GetComponent<MotionHandler>();
        if (player_num % 2 == 0)
            facing = 1;
        else
        {
            flip();
            facing = -1;
        }
        
        motionHandler.ChangeYSpeed(0);
        jumps = max_jumps;
        
        game_controller = BattleController.current_battle;

        //Load SFX
        string directory = Path.Combine("Assets/Resources/"+resource_path, sound_path);
        DirectoryInfo directory_info = new DirectoryInfo(directory);
        if (directory_info.Exists)
        {
            foreach (FileInfo filename in directory_info.GetFiles())
            {
                if (filename.Extension != ".meta")
                {
                    string name_no_ext = filename.Name.Split('.')[0];
                    sounds.Add(name_no_ext, Resources.Load<AudioClip>(resource_path + sound_path + name_no_ext));
                }
                //Resources.Load<AudioClip>(filename);
            }
        }
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
        //Set gravity, or reset jumps
        if (grounded)
        {
            jumps = max_jumps;
        }
        else
        {
            motionHandler.ChangeYSpeedBy(gravity * 5 * Time.deltaTime);
            if (motionHandler.YSpeed < max_fall_speed || (motionHandler.YSpeed < 0 && GetControllerAxis("Vertical") < -0.3))
            {
                motionHandler.ChangeYSpeed(max_fall_speed);
            } 
        }

        //Set horizontal and vertical deltas
        float current_x = GetControllerAxis("Horizontal");
        x_axis_delta = Mathf.Abs(current_x) - Mathf.Abs(last_x_axis);
        last_x_axis = current_x;

        float current_y = GetControllerAxis("Vertical");
        y_axis_delta = Mathf.Abs(current_y) - Mathf.Abs(last_y_axis);
        last_y_axis = current_y;

        //Enable Phasing
        if (GetControllerAxis("Vertical") < -0.3)
            platform_phaser.EnableDownPhase = true;
        else
            platform_phaser.EnableDownPhase = false;

        battleObject.ManualUpdate();
        
        if (grounded)
            battleObject.GetMotionHandler().accel(friction);
        else
            battleObject.GetMotionHandler().accel(air_resistance);

        
    }

    public void doAction(string _actionName)
    {
        BroadcastMessage("DoAction", _actionName);
    }

    /// <summary>
    /// Gets the horizontal direction relative to the direction of facing, so that positive is forward and negative is backward.
    /// </summary>
    /// <returns> The float value of the direction relative to facing</returns>
    public float GetDirectionRelative()
    {
        return GetControllerAxis("Horizontal") * facing;
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
            if (KeyBuffered(InputTypeUtil.GetForward(this), 2))
                doAction("ForwardSmash");
            else
                doAction("ForwardAttack");
        else if (GetDirectionRelative() < 0.0f)
        {
            flip();
            if (KeyBuffered(InputTypeUtil.GetForward(this), 2))
                doAction("ForwardSmash");
            else
                doAction("ForwardAttack");
        }
        else if (GetControllerAxis("Vertical") > 0.0f)
            if (KeyBuffered(InputType.Up, 2))
                doAction("UpSmash");
            else
                doAction("UpAttack");
        else if (GetControllerAxis("Vertical") < 0.0f)
            if (KeyBuffered(InputType.Down, 2))
                doAction("DownSmash");
            else
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
    
    public void PlaySound(string sound_name)
    {
        if (sounds.ContainsKey(sound_name))
            sound_player.PlayOneShot(sounds[sound_name]);
    }
    /// <summary>
    /// If the Hitbox is not "locked" from the fighter, locks it and returns True, allow it to affect the fighter.
    /// If it is locked, returns false.
    /// </summary>
    /// <param name="hitbox"></param>
    /// <returns></returns>
    public bool LockHitbox(Hitbox hitbox)
    {
        if (hitbox_locks.Contains(hitbox.hitbox_lock)) //If it's in the locks, return false and do nothing else.
            return false;
        else
        {
            hitbox.hitbox_lock.PutInList(hitbox_locks);
            StartCoroutine(RemoveLock(hitbox.hitbox_lock));
            return true;
        }

    }

    public void GetHit(Hitbox hitbox)
    {
        if (LockHitbox(hitbox)) //If the hitbox is not already locked to us
        {
            float weight_constant = 1.4f;
            float flat_constant = 5.0f;

            float percent_portion = (damage_percent / 10.0f) + ((damage_percent * hitbox.damage) / 20.0f);
            float weight_portion = 200.0f / (weight * hitbox.weight_influence + 100);
            float scaled_kb = (((percent_portion * weight_portion * weight_constant) + flat_constant) * hitbox.knockback_growth);
            ApplyKnockback(scaled_kb + hitbox.base_knockback, hitbox.trajectory);
            DealDamage(hitbox.damage);
        }
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
        battleObject.BroadcastMessage("ChangeXSpeed",trajectory_vec.x);
        battleObject.BroadcastMessage("ChangeYSpeed", trajectory_vec.y);
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

    /*
    public void TestActionJSON()
    {
        string action_json_path = Path.Combine("Assets/Resources/"+resource_path, action_file);
        if (File.Exists(action_json_path))
        {
            string action_json = File.ReadAllText(action_json_path);
            Debug.Log(action_json);
            actions_file_json = JsonUtility.FromJson<ActionFile>(action_json);
        }
        File.WriteAllText(action_json_path, JsonUtility.ToJson(actions_file_json, true));
    }
    */

    /////////////////////////////////////////////////////////////////////////////////////////
    //                              PRIVATE HELPER METHODS                                 //
    /////////////////////////////////////////////////////////////////////////////////////////

    private IEnumerator RemoveLock(HitboxLock hitbox_lock)
    {
        yield return new WaitForSeconds(2);
        if (hitbox_locks.Contains(hitbox_lock)) //It can be unlocked later
            hitbox_locks.Remove(hitbox_lock);
    }

    /////////////////////////////////////////////////////////////////////////////////////////
    //                               BROADCAST RECEIVERS                                   //
    /////////////////////////////////////////////////////////////////////////////////////////
    void SetGrounded(bool groundedval)
    {
        grounded = groundedval;
    }
}