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

    public int player_num = 0;

    [HideInInspector]
    public float ground_elasticity = 0.0f, damage_percent = 0;
    
    private FighterInfo fighter_info;
    private SpriteHandler sprite_loader;
    private Animator anim;
    private InputBuffer inputBuffer;
    private PlatformPhase platform_phaser;
    private AudioSource sound_player;
    private Dictionary<string, AudioClip> sounds = new Dictionary<string, AudioClip>();
    private List<HitboxLock> hitbox_locks = new List<HitboxLock>();
    private Collider coll;
    private List<Collider> contacted_colliders = new List<Collider>();
    private List<Ledge> contacted_ledges = new List<Ledge>();
    public AbstractFighter hitTagged = null;
    public bool LedgeLock = false;


    void LoadInfo()
    {
        fighter_info = GetComponent<FighterInfoLoader>().GetFighterInfo();
        foreach(KeyValuePair<string,object> variable in DefaultStats)
        {
            SetVar(variable.Key, variable.Value);
        }

        foreach(VarData variable in fighter_info.variables)
        {
            VarType type = variable.type;
            switch (type)
            {
                case VarType.BOOL:
                    SetVar(variable.name, bool.Parse(variable.value));
                    break;
                case VarType.FLOAT:
                    SetVar(variable.name, float.Parse(variable.value));
                    break;
                case VarType.INT:
                    SetVar(variable.name, int.Parse(variable.value));
                    break;
                default:
                    SetVar(variable.name, variable.value);
                    break;
            }
        }
    }

    /// <summary>
    /// There are several pieces of AbstractFighter that are broken off into other scripts for
    /// the sake of code organization. Make sure they're all loaded here.
    /// </summary>
    private void LoadComponents()
    {
        inputBuffer = battleObject.GetInputBuffer();

        platform_phaser = GetComponent<PlatformPhase>();
        if (platform_phaser == null)
            platform_phaser = gameObject.AddComponent<PlatformPhase>();

        sprite_loader = GetComponent<SpriteHandler>();
        anim = GetComponent<Animator>();
        sound_player = GetComponent<AudioSource>();
        coll = GetComponent<Collider>();
    }

    void SetVariables()
    {
        SetVar("jumps", 0);
        SetVar("facing", 1);
        SetVar("landing_lag", 0);
        SetVar("tech_window", 0);
        SetVar("air_dodges", 1);
        SetVar("grounded", false);
        SetVar("elasticity", 0.0f);

        //Change variables according to Settings
        Settings settings = Settings.current_settings;
        SetVar("gravity", GetFloatVar("gravity") * settings.gravity_ratio);
        SetVar("weight", GetFloatVar("weight") * settings.weight_ratio);
        SetVar("friction", GetFloatVar("friction") * settings.friction_ratio);
        SetVar("air_control", GetFloatVar("air_control") * settings.aircontrol_ratio);
    }

    void Start() {
        LoadComponents();
        LoadInfo();
        SetVariables();
        
        SetVar("facing", 1);

        SendMessage("ChangeYSpeed", 0f);
        Rest();

        //Load SFX
        string directory = FileLoader.PathCombine(FileLoader.GetFighterPath(fighter_info.directory_name),fighter_info.sound_path);
        DirectoryInfo directory_info = new DirectoryInfo(directory);
        if (directory_info.Exists)
        {
            foreach (FileInfo filename in directory_info.GetFiles())
            {
                //Ignore Unity Meta files
                if (filename.Extension != ".meta")
                {
                    string name_no_ext = filename.Name.Split('.')[0];
                    AudioClip audio = Resources.Load<AudioClip>("Fighters/" + fighter_info.directory_name + "/" + fighter_info.sound_path + "/" + name_no_ext);
                    sounds.Add(name_no_ext, audio);
                }
            }
        }
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

        //Enable Phasing
        if (CheckSmash("DownSmash"))
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
        if (DirectionHeld("Forward"))
        {
            if (CheckSmash("ForwardSmash"))
                doAction("ForwardSmash");
            else doAction("ForwardAttack");
        }
        else if (DirectionHeld("Backward"))
        {
            battleObject.SendMessage("flip");
            if (CheckSmash("BackwardSmash"))
                doAction("BackwardSmash");
            else doAction("ForwardAttack");
        }
        else if (DirectionHeld("Up"))
        {
            if (CheckSmash("UpSmash"))
                doAction("UpSmash");
            else doAction("UpAttack");
        }
            
        else if (DirectionHeld("Down"))
        {
            if (CheckSmash("DownSmash"))
                doAction("DownSmash");
            else doAction("DownAttack");
        }
        else
            doAction("NeutralAttack");
    }

    public void doGroundSpecial()
    {
        if (DirectionHeld("Forward"))
            doAction("ForwardSpecial");
        else if (DirectionHeld("Backward"))
        {
            battleObject.SendMessage("flip");
            doAction("ForwardSpecial");
        }
        else if (DirectionHeld("Up"))
            doAction("UpSpecial");
        else if (DirectionHeld("Down"))
            doAction("DownSpecial");
        else
            doAction("NeutralSpecial");
    }

    public void doAirAttack()
    {
        if (DirectionHeld("Forward"))
            doAction("ForwardAir");
        else if (DirectionHeld("Backward"))
            doAction("BackAir");
        else if (DirectionHeld("Up"))
            doAction("UpAir");
        else if (DirectionHeld("Down"))
            doAction("DownAir");
        else
            doAction("NeutralAir");
    }

    public void doAirSpecial()
    {
        if (DirectionHeld("Forward"))
            doAction("ForwardSpecial");
        else if (DirectionHeld("Backward"))
        {
            SendMessage("flip");
            doAction("ForwardSpecial");
        }
        else if (DirectionHeld("Up"))
            doAction("UpSpecial");
        else if (DirectionHeld("Down"))
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
        frames *= Settings.current_settings.hitlag_ratio;
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

        //Apply hitstun scaling
        hitstun_frames *= Settings.current_settings.hitstun_ratio;

        if (hitstun_frames > 0.5)
        {
            //if not isinstance(self.current_action, baseActions.HitStun) or (self.current_action.last_frame-self.current_action.frame) / float(settingsManager.getSetting('hitstun')) <= hitstun_frames+15:
            DoHitStun(hitstun_frames * Settings.current_settings.hitstun_ratio, _trajectory);
            battleObject.GetActionHandler().CurrentAction.SetVar("tech_cooldown", Mathf.RoundToInt(_total_kb * _hitstunMultiplier));
        }
    }

    public bool KeyBuffered(string key, int distance = 12, bool pressed = true)
    {
        return inputBuffer.KeyBuffered(key, distance, pressed);
    }

    public bool CheckBuffer(string key, int distance = 12, bool pressed = true)
    {
        return inputBuffer.CheckBuffer(key, distance, pressed);
    }

    public bool KeyHeld(string key)
    {
        return inputBuffer.GetKey(key);
    }

    public bool DirectionHeld(string direction)
    {
        return inputBuffer.DirectionHeld(direction);
    }

    public float GetAxis(string axis)
    {
        return inputBuffer.GetAxis(axis);
    }

    /*public bool SequenceBuffered(List<KeyValuePair<InputType,float>> inputList, int distance = 12)
    {
        return inputBuffer.SequenceBuffered(inputList, distance);
    }*/


    public bool CheckSmash(string key)
    {
        if (key == "ForwardSmash")
        {
            if (GetIntVar("facing") == 1) key = "RightSmash";
            else key = "LeftSmash";
        }
        if (key == "BackwardSmash")
        {
            if (GetIntVar("facing") == 1) key = "LeftSmash";
            else key = "RightSmash";
        }
        return inputBuffer.KeyBuffered(key);
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

    public void SetPlayerNum(int playernum)
    {
        player_num = playernum;
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