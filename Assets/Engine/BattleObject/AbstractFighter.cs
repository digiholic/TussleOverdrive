using System.Collections;
using System.Collections.Generic;
using System.Xml;
using System.IO;
using UnityEngine;
using UnityEngine.Profiling;

[System.Serializable]
public class AbstractFighter : BattleComponent {
    private static Dictionary<string, object> DefaultStats = new Dictionary<string, object>
    {
        {"weight", 10.0f },
        {"gravity", -9.8f},
        {"max_fall_speed", -20.0f},
        {"max_ground_speed", 7.0f},
        {"run_speed", 11.0f},
        {"max_air_speed", 5.5f},
        {"crawl_speed", 2.5f},
        {"dodge_sepeed", 8.5f},
        {"friction", 0.3f},
        {"static_grip", 0.3f},
        {"pivot_grip", 0.6f},
        {"air_resistance", 0.2f},
        {"air_control", 0.2f},
        {"jump_height", 15.0f},
        {"short_hop_height", 8.5f},
        {"air_jump_height", 18.0f},
        {"fastfall_multiplier", 2.0f},
        {"hitstun_elasticity", 0.8f},
        {"shield_size", 1.0f},
        {"aerial_transition_speed", 9.0f},
        {"pixels_per_unit", 100},
        {"max_jumps", 1 },
        {"heavy_land_lag", 4 },
        {"wavedash_lag", 12 }
    };


    //public string fighter_xml_file = "";
    private string resource_path = "";
    public int player_num = 0;

    public string fighter_name = "Unknown";
    public string franchise_icon = "Assets/Sprites/Defaults/franchise_icon.png";
    public string css_icon = "Assets/Sprites/Defaults/css_icon.png";
    public string css_portrait = "Assets/Sprites/Default/css_portrait.png";

    [HideInInspector]
    public string sound_path = "";

    void SetVariables()
    {
        SetVar("jumps", 0);
        SetVar("facing", 1);
        SetVar("landing_lag", 0);
        SetVar("tech_window", 0);
        SetVar("air_dodges", 1);
        SetVar("grounded", false);
    }
    [HideInInspector]
    public float ground_elasticity = 0.0f, damage_percent = 0;
    
    [HideInInspector]
    public BattleController game_controller;
    
    private SpriteHandler sprite_loader;
    private Animator anim;
    private InputManager inputBuffer;
    private XMLLoader data_xml;
    private AudioSource sound_player;
    private PlatformPhase platform_phaser;
    private Collider coll;
    public BattleObject BattleObject { get { return battleObject; } set { battleObject = value; } }

    private float last_x_axis;
    private float x_axis_delta;
    private float last_y_axis;
    private float y_axis_delta;
    private List<HitboxLock> hitbox_locks = new List<HitboxLock>();
    private Dictionary<string, AudioClip> sounds = new Dictionary<string, AudioClip>();

    private List<Collider> contacted_colliders = new List<Collider>();
    private List<Ledge> contacted_ledges = new List<Ledge>();
    public AbstractFighter hitTagged = null;

    public bool LedgeLock { get; set; }


    void LoadFighterXML()
    {
        data_xml = GetComponent<XMLLoader>();
        resource_path = data_xml.resource_path;   

        if (data_xml != null)
        {
            fighter_name = data_xml.SelectSingleNode("//fighter/name").GetString();
            franchise_icon = data_xml.SelectSingleNode("//fighter/icon").GetString();
            css_icon = data_xml.SelectSingleNode("//fighter/css_icon").GetString();
            css_portrait = data_xml.SelectSingleNode("//fighter/css_portrait").GetString();
            
            sound_path = data_xml.SelectSingleNode("//fighter/sound_path").GetString();
            
            /*
            SetVar("fighter_name", data_xml.SelectSingleNode("//fighter/name").GetString());
            SetVar("franchise_icon", data_xml.SelectSingleNode("//fighter/icon").GetString());
            SetVar("css_icon", data_xml.SelectSingleNode("//fighter/css_icon").GetString());
            SetVar("css_portrait", data_xml.SelectSingleNode("//fighter/css_portrait").GetString());

            article_path = data_xml.SelectSingleNode("//fighter/article_path").GetString();
            article_file = data_xml.SelectSingleNode("//fighter/articles").GetString();
            sound_path = data_xml.SelectSingleNode("//fighter/sound_path").GetString();

            action_file = data_xml.SelectSingleNode("//fighter/actions").GetString();
            */

            //Load the stats
            SetVar("weight", data_xml.SelectSingleNode("//fighter/stats/weight").GetString());
            SetVar("gravity", data_xml.SelectSingleNode("//fighter/stats/gravity").GetString());
            SetVar("max_fall_speed", data_xml.SelectSingleNode("//fighter/stats/max_fall_speed").GetString());
            SetVar("max_ground_speed", data_xml.SelectSingleNode("//fighter/stats/max_ground_speed").GetString());
            SetVar("run_speed", data_xml.SelectSingleNode("//fighter/stats/run_speed").GetString());
            SetVar("max_air_speed", data_xml.SelectSingleNode("//fighter/stats/max_air_speed").GetString());
            SetVar("aerial_transition_speed", data_xml.SelectSingleNode("//fighter/stats/aerial_transition_speed").GetString());
            SetVar("crawl_speed", data_xml.SelectSingleNode("//fighter/stats/crawl_speed").GetString());
            SetVar("dodge_speed", data_xml.SelectSingleNode("//fighter/stats/dodge_speed").GetString());
            SetVar("friction", data_xml.SelectSingleNode("//fighter/stats/friction").GetString());
            SetVar("static_grip", data_xml.SelectSingleNode("//fighter/stats/static_grip").GetString());
            SetVar("pivot_grip", data_xml.SelectSingleNode("//fighter/stats/pivot_grip").GetString());
            SetVar("air_resistance", data_xml.SelectSingleNode("//fighter/stats/air_resistance").GetString());
            SetVar("air_control", data_xml.SelectSingleNode("//fighter/stats/air_control").GetString());
            SetVar("jump_height", data_xml.SelectSingleNode("//fighter/stats/jump_height").GetString());
            SetVar("short_hop_height", data_xml.SelectSingleNode("//fighter/stats/short_hop_height").GetString());
            SetVar("air_jump_height", data_xml.SelectSingleNode("//fighter/stats/air_jump_height").GetString());
            SetVar("fastfall_multiplier", data_xml.SelectSingleNode("//fighter/stats/fastfall_multiplier").GetString());

            SetVar("hitstun_elasticity", data_xml.SelectSingleNode("//fighter/stats/hitstun_elasticity").GetString());
            SetVar("shield_size", data_xml.SelectSingleNode("//fighter/stats/shield_size").GetString());
            SetVar("max_jumps", data_xml.SelectSingleNode("//fighter/stats/jumps").GetString());
            SetVar("heavy_landing_lag", data_xml.SelectSingleNode("//fighter/stats/heavy_land_lag").GetString());
            SetVar("wavedash_lag", data_xml.SelectSingleNode("//fighter/stats/wavedash_lag").GetString());


        }
        else
        {
            throw new System.Exception("Could not load FighterXML");
        }
    }

    /// <summary>
    /// There are several pieces of AbstractFighter that are broken off into other scripts for
    /// the sake of code organization. Make sure they're all loaded here.
    /// </summary>
    private void LoadComponents()
    {
        inputBuffer = GetComponent<InputManager>();
        if (inputBuffer == null)
        {
            inputBuffer = gameObject.AddComponent<InputManager>();
            inputBuffer.player_num = player_num;
            inputBuffer.LoadAllKeys();
        }
            

        platform_phaser = GetComponent<PlatformPhase>();
        if (platform_phaser == null)
            platform_phaser = gameObject.AddComponent<PlatformPhase>();
    }

    void Start() {
        LoadComponents();
        LoadFighterXML();
        SetVariables();
        sprite_loader = GetComponent<SpriteHandler>();
        sprite_loader.Initialize();
        anim = GetComponent<Animator>();
        sound_player = GetComponent<AudioSource>();
        coll = GetComponent<Collider>();

        if (player_num % 2 == 0)
            SetVar("facing", 1);
        
        else
        {
            battleObject.SendMessage("flip");
            SetVar("facing", -1);
        }
        
        SendMessage("ChangeYSpeed", 0f);
        SetVar("jumps",GetIntVar("max_jumps"));
        SetVar("elasticity", 0.0f);

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

        //Debug.Log(JsonUtility.ToJson(this));
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
    public override void ManualUpdate () {
        //Set gravity, or reset jumps
        if (GetBoolVar("grounded"))
            Rest();
        else
        {
            SendMessage("CalcGrav",new float[] { GetFloatVar("gravity"), GetFloatVar("max_fall_speed") });
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

        if (GetBoolVar("grounded"))
            battleObject.GetMotionHandler().accel(GetFloatVar("friction"));
        else
            battleObject.GetMotionHandler().accel(GetFloatVar("air_resistance"));
    }

    public void WallBounce(ControllerColliderHit hit)
    {
        //Impact article generation
        GameObject impact = ObjectPooler.current_pooler.GetPooledObject("Impact");
        impact.transform.position = hit.point;
        impact.transform.eulerAngles = new Vector3(0, 0, 90); //Reset position to rotate properly
        impact.transform.rotation = Quaternion.FromToRotation(impact.transform.up, hit.normal);
        impact.SetActive(true);
        impact.SendMessage("Burst");

        MotionHandler mot = battleObject.GetMotionHandler();

        Vector3 refVector = Vector3.Reflect(mot.GetMotionVector(), hit.normal);
        transform.Translate(refVector.normalized * 0.3f);
        mot.ChangeSpeedVector(refVector * GetFloatVar("elasticity"));

        //rotate to the new bounce direction
        Vector2 directMagn = mot.GetDirectionMagnitude();
        SendMessage("UnRotate");
        SendMessage("RotateSprite", (directMagn.x - 90) * GetIntVar("facing"));

        SetVar("StopFrames", 5);
    }


    public void Die()
    {
        transform.position = new Vector3(0, 10);
        //TODO send death signal, handle respawning in-object
        damage_percent = 0;
        SendMessage("ChangeXSpeed", 0.0f);
        SendMessage("ChangeYSpeed", 0.0f);
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
        return GetControllerAxis("Horizontal") * battleObject.GetIntVar("facing");
    }

    public void flip()
    {
        if (battleObject.GetSpriteHandler() != null) //Sprites get flipped
            transform.localScale = new Vector3(-transform.localScale.x, transform.localScale.y, transform.localScale.z);
        if (battleObject.GetModelHandler() != null) //Models get rotated
            transform.Rotate(transform.rotation.x, 180, transform.rotation.z);
        SetVar("facing", -1 * battleObject.GetIntVar("facing"));
    }

    public void doGroundAttack()
    {
        if (GetDirectionRelative() > 0.0f)
            if (KeyBuffered(InputTypeUtil.GetForward(battleObject), 2))
                doAction("ForwardSmash");
            else
                doAction("ForwardAttack");
        else if (GetDirectionRelative() < 0.0f)
        {
            battleObject.SendMessage("flip");
            if (KeyBuffered(InputTypeUtil.GetForward(battleObject), 2))
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

    public void doGroundSpecial()
    {
        if (GetDirectionRelative() > 0.0f)
            doAction("ForwardSpecial");
        else if (GetDirectionRelative() < 0.0f)
        {
            battleObject.SendMessage("flip");
            doAction("ForwardSpecial");
        }
        else if (GetControllerAxis("Vertical") > 0.0f)
            doAction("UpSpecial");
        else if (GetControllerAxis("Vertical") < 0.0f)
            doAction("DownSpecial");
        else
            doAction("NeutralSpecial");
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

    public void doAirSpecial()
    {
        if (GetDirectionRelative() > 0.0f)
            doAction("ForwardSpecial");
        else if (GetDirectionRelative() < 0.0f)
        {
            SendMessage("flip");
            doAction("ForwardSpecial");
        }
        else if (GetControllerAxis("Vertical") > 0.0f)
            doAction("UpSpecial");
        else if (GetControllerAxis("Vertical") < 0.0f)
            doAction("DownSpecial");
        else
            doAction("NeutralSpecial");
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
        {
            return false;
        }
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
            hitTagged = hitbox.owner.GetAbstractFighter();
            StartCoroutine(RemoveTag());

            float weight_constant = 1.4f;
            float flat_constant = 5.0f;

            float percent_portion = (damage_percent / 10.0f) + ((damage_percent * hitbox.damage) / 20.0f);
            float weight_portion = 200.0f / (GetFloatVar("weight") * hitbox.weight_influence + 100);
            float scaled_kb = (((percent_portion * weight_portion * weight_constant) + flat_constant) * hitbox.knockback_growth);
            ApplyKnockback(scaled_kb + hitbox.base_knockback, hitbox.trajectory);
            DealDamage(hitbox.damage);
            ApplyHitstop(hitbox.damage);//TODO hitlag multiplier
            if (hitbox.owner != null) hitbox.owner.SendMessage("ApplyHitstop", hitbox.damage);
            ApplyHitstun(scaled_kb + hitbox.base_knockback, 1.0f, 1.0f, hitbox.trajectory);
        }
    }

    public void DealDamage(float _damage)
    {
        damage_percent += _damage;
        damage_percent = Mathf.Min(999, damage_percent);

        //TODO log damage data
    }

    public void ApplyHitstop(float frames)
    {
        //battleObject.PrintDebug(this, 2, "Applying Hitstop of "+frames.ToString()+" frames");
        SetVar("StopFrames", Mathf.FloorToInt(frames));
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
        float hitstun_frames = Mathf.Floor((_total_kb) * _hitstunMultiplier + _baseHitstun);

        if (hitstun_frames > 0.5)
        {
            //if not isinstance(self.current_action, baseActions.HitStun) or (self.current_action.last_frame-self.current_action.frame) / float(settingsManager.getSetting('hitstun')) <= hitstun_frames+15:
            DoHitStun(hitstun_frames * Settings.current_settings.preset.hitstun_ratio, _trajectory);
            battleObject.GetActionHandler().CurrentAction.SetVar("tech_cooldown", Mathf.RoundToInt(_total_kb * _hitstunMultiplier));
        }
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

    public bool KeyBuffered(InputType key, int distance = 12, float threshold = 1.0f)
    {
        if (key == InputType.Forward) key = InputTypeUtil.GetForward(battleObject);
        if (key == InputType.Backward) key = InputTypeUtil.GetBackward(battleObject);
        return inputBuffer.KeyBuffered(key, distance, threshold);
    }

    public bool CheckBuffer(InputType key, int distance = 12, float threshold = 1.0f)
    {
        if (key == InputType.Forward) key = InputTypeUtil.GetForward(battleObject);
        if (key == InputType.Backward) key = InputTypeUtil.GetBackward(battleObject);
        return inputBuffer.CheckBuffer(key, distance, threshold);
    }

    public bool KeyHeld(InputType key)
    {
        if (key == InputType.Forward) key = InputTypeUtil.GetForward(battleObject);
        if (key == InputType.Backward) key = InputTypeUtil.GetBackward(battleObject);
        return inputBuffer.GetKey(key);
    }

    /*public bool SequenceBuffered(List<KeyValuePair<InputType,float>> inputList, int distance = 12)
    {
        return inputBuffer.SequenceBuffered(inputList, distance);
    }*/


    public bool CheckSmash(InputType key)
    {
        if (key == InputType.Forward) key = InputTypeUtil.GetForward(battleObject);
        if (key == InputType.Backward) key = InputTypeUtil.GetBackward(battleObject);

        return inputBuffer.CheckDoubleTap(key,32);
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
    //                                    TRIGGERS                                         //
    /////////////////////////////////////////////////////////////////////////////////////////
    void OnTriggerEnter(Collider other)
    {
        if (!other.transform.IsChildOf(this.transform))
        {
            contacted_colliders.Add(other);
            battleObject.PrintDebug(this, 3, "Enterred Collider " + other);
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (!other.transform.IsChildOf(this.transform))
        {
            if (contacted_colliders.Contains(other))
                contacted_colliders.Remove(other);
            battleObject.PrintDebug(this, 3, "Left Collider " + other);
        }

        
        if (other.tag == "Killbox" && !other.bounds.Contains(transform.position))
        {
            GameObject deathBurst = ObjectPooler.current_pooler.GetPooledObject("DeathBurst");
            deathBurst.transform.position = transform.position;
            //TODO change color to that of the player that kill
            deathBurst.SetActive(true);

            Color deathCol = Settings.current_settings.player_colors[player_num];
            if (hitTagged != null) deathCol = Settings.current_settings.player_colors[hitTagged.player_num];
            deathBurst.SendMessage("ChangeColor", deathCol);

            deathBurst.SendMessage("Burst");
            SetVar("StopFrames", 60);
            doAction("Fall"); //TODO respawn
            Die();
        }
        
    }

    public List<Collider> GetCollisionsWithLayer(string layer)
    {
        List<Collider> retlist = new List<Collider>();
        foreach (Collider col in contacted_colliders)
        {
            if (col.gameObject.layer == LayerMask.NameToLayer(layer))
                retlist.Add(col);
        }
        return retlist;
    }

    /////////////////////////////////////////////////////////////////////////////////////////
    //                              PRIVATE HELPER METHODS                                 //
    /////////////////////////////////////////////////////////////////////////////////////////

    private IEnumerator RemoveLock(HitboxLock hitbox_lock)
    {
        yield return new WaitForSeconds(2);
        if (hitbox_locks.Contains(hitbox_lock)) //It can be unlocked later
            hitbox_locks.Remove(hitbox_lock);
    }

    private IEnumerator RemoveTag()
    {
        yield return new WaitForSeconds(300);
        hitTagged = null;
    }

    private IEnumerator WaitForFrames(int frames, string command)
    {
        int targetFrame = BattleController.current_battle.current_game_frame + frames;

        while (BattleController.current_battle.current_game_frame < targetFrame) {
            Debug.Log("Waiting...");
            yield return null;
        }
        SendMessage(command);
    }

    public class WaitForUnlock : CustomYieldInstruction
    {
        private int clockTarget;
        public override bool keepWaiting
        {
            get
            {
                return BattleController.current_battle.current_game_frame < clockTarget;
            }
        }

        public WaitForUnlock(int frameCount)
        {
            clockTarget = BattleController.current_battle.current_game_frame + frameCount;
        }
    }
    /////////////////////////////////////////////////////////////////////////////////////////
    //                               BROADCAST RECEIVERS                                   //
    /////////////////////////////////////////////////////////////////////////////////////////

    /// <summary>
    /// Recharge everything that happens on a "Rest", restoring jumps, airdodges, etc.
    /// </summary>
    void Rest()
    {
        SetVar("jumps", GetIntVar("max_jumps"));
        SetVar("air_dodges", 1); //TODO change this based on settings

    }

    private Ledge grabbed_ledge;
    public Ledge GrabbedLedge { get { return grabbed_ledge; } }
    
    public void EnterLedge(Ledge ledge)
    {
        contacted_ledges.Add(ledge);
    }

    public void ExitLedge(Ledge ledge)
    {
        if (contacted_ledges.Contains(ledge))
        {
            contacted_ledges.Remove(ledge);
            if (contacted_ledges.Count == 0) LedgeLock = false;
        }   
    }

    public List<Ledge> GetLedges()
    {
        return contacted_ledges;
    }

    public void GrabLedge(Ledge ledgeToGrab)
    {
        grabbed_ledge = ledgeToGrab;
        LedgeLock = true;
        doAction("LedgeGrab");
    }

    public void ReleaseLedge()
    {
        grabbed_ledge = null;
    }

    public void GetTrumped(Ledge ledgeTrumpedFrom)
    {
        Debug.Log("CAN'T STUMP THE LEDGE TRUMP");
    }

    /////////////////////////////////////////////////////////////////////////////////////////
    //                                  ACTION SETTERS                                     //
    /////////////////////////////////////////////////////////////////////////////////////////

    void DoHitStun(float hitstun, float trajectory)
    {
        doAction("HitStun");
        battleObject.GetActionHandler().CurrentAction.SetVar("angle", trajectory);
        battleObject.GetActionHandler().CurrentAction.AdjustLength(Mathf.RoundToInt(hitstun));

    }
}