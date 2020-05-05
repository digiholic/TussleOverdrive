using System.Collections;
using System.Collections.Generic;
using System.Xml;
using System.IO;
using UnityEngine;
using UnityEngine.Profiling;

[System.Serializable]
[RequireComponent(typeof(ActionHandler),typeof(InputBuffer))]
public class AbstractFighter : BattleComponent {
    [HideInInspector]
    public float ground_elasticity = 0.0f, damage_percent = 0;

    public int player_num = 0;

    private FighterInfo fighter_info;
    private SpriteHandler sprite_loader;
    private Animator anim;
    private InputBuffer inputBuffer;
    //FIXME: maybe this action handler reference isn't needed? It's only used for setting vars
    private ActionHandler actionHandler;
    private PlatformPhase platform_phaser;
    private AudioSource sound_player;
    private Dictionary<string, AudioClip> sounds = new Dictionary<string, AudioClip>();
    private List<HitboxLock> hitbox_locks = new List<HitboxLock>();
    private Collider coll;
    private List<Collider> contacted_colliders = new List<Collider>();
    private List<Ledge> contacted_ledges = new List<Ledge>();
    public AbstractFighter hitTagged = null;
    private IEnumerator hitTagCoroutine = null;
    public bool LedgeLock = false;

    void LoadInfo()
    {
        if (fighter_info == null) fighter_info = GetComponent<FighterInfoLoader>().GetFighterInfo();

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
        inputBuffer = GetComponent<InputBuffer>();
        actionHandler = GetComponent<ActionHandler>();

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
        SetVar(TussleConstants.FighterVariableNames.PLAYER_NUM, player_num);

        SetVar(TussleConstants.FighterVariableNames.JUMPS_REMAINING, 0);
        SetVar(TussleConstants.FighterVariableNames.FACING_DIRECTION, 1);
        SetVar(TussleConstants.FighterVariableNames.LANDING_LAG, 0);
        SetVar(TussleConstants.FighterVariableNames.TECH_WINDOW, 0);
        SetVar(TussleConstants.FighterVariableNames.AIR_DODGES_REMAINING, 1);
        SetVar(TussleConstants.FighterVariableNames.IS_GROUNDED, false);
        SetVar(TussleConstants.FighterVariableNames.ELASTICITY, 0.0f);

        SetVar(TussleConstants.ColliderVariableNames.IS_PHASING,false);
        
        //Change variables according to Settings
        Settings settings = Settings.current_settings;
        //SetVar(TussleConstants.FighterAttributes.GRAVITY, GetFloatVar(TussleConstants.FighterAttributes.GRAVITY) * settings.gravity_ratio);
        //SetVar(TussleConstants.FighterAttributes.WEIGHT, GetFloatVar(TussleConstants.FighterAttributes.WEIGHT) * settings.weight_ratio);
        //SetVar(TussleConstants.FighterAttributes.FRICTION, GetFloatVar(TussleConstants.FighterAttributes.FRICTION) * settings.friction_ratio);
        //SetVar(TussleConstants.FighterAttributes.AIR_CONTROL, GetFloatVar(TussleConstants.FighterAttributes.AIR_CONTROL) * settings.aircontrol_ratio);
    }

    private void Awake()
    {
        LoadComponents();
        LoadInfo();
        SetVariables();
    }

    void Start() {
        SetVar(TussleConstants.FighterVariableNames.FACING_DIRECTION, 1);
        
        SendMessage("ChangeYSpeed", 0.0f, SendMessageOptions.RequireReceiver);
        Rest();

        //Load SFX
        string directory = FileLoader.PathCombine(FileLoader.GetFighterPath(fighter_info.directory_name),fighter_info.soundPath);
        DirectoryInfo directory_info = new DirectoryInfo(directory);
        if (directory_info.Exists)
        {
            foreach (FileInfo filename in directory_info.GetFiles())
            {
                //Ignore Unity Meta files
                if (filename.Extension != ".meta")
                {
                    string name_no_ext = filename.Name.Split('.')[0];
                    AudioClip audio = Resources.Load<AudioClip>("Fighters/" + fighter_info.directory_name + "/" + fighter_info.soundPath + "/" + name_no_ext);
                    sounds.Add(name_no_ext, audio);
                }
            }
        }
    }

    // Update is called once per frame
    public override void ManualUpdate () {
        //Set gravity, or reset jumps
        if (GetBoolVar(TussleConstants.FighterVariableNames.IS_GROUNDED))
            Rest();
        else
        {
            SendMessage("CalcGrav",new float[] { GetFloatVar(TussleConstants.FighterAttributes.GRAVITY), GetFloatVar(TussleConstants.FighterAttributes.MAX_FALL_SPEED) });
        }

        //Enable Phasing if the joystick is smashed down
        platform_phaser.EnableDownPhase = CheckSmash("DownSmash");

        if (GetBoolVar(TussleConstants.FighterVariableNames.IS_GROUNDED))
            SendMessage("accel",GetFloatVar(TussleConstants.FighterAttributes.FRICTION));
        else
            SendMessage("accel", GetFloatVar(TussleConstants.FighterAttributes.AIR_RESISTANCE));

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

        float XSpeed = GetFloatVar(TussleConstants.MotionVariableNames.XSPEED);
        float YSpeed = GetFloatVar(TussleConstants.MotionVariableNames.YSPEED);
        Vector3 motVector = new Vector3(XSpeed, YSpeed, 0.0f); ;
        Vector3 refVector = Vector3.Reflect(motVector, hit.normal);
        transform.Translate(refVector.normalized * 0.3f);
        SendMessage("ChangeSpeedVector", refVector * GetFloatVar(TussleConstants.FighterVariableNames.ELASTICITY));

        //rotate to the new bounce direction
        Vector2 directMagn = MotionHandler.GetDirectionMagnitude(getBattleObject());
        SendMessage("UnRotate");
        SendMessage("RotateSprite", (directMagn.x - 90) * GetIntVar(TussleConstants.FighterVariableNames.FACING_DIRECTION));

        SetVar("StopFrames", 5);
    }


    public void Die()
    {
        BattleController.current_battle.FighterDies(this,hitTagged);
        //TODO send death signal, handle respawning in-object
    }

    public void Respawn(){
        transform.position = new Vector3(0, 10);
        damage_percent = 0;
        SendMessage("ChangeXSpeed", 0.0f, SendMessageOptions.RequireReceiver);
        SendMessage("ChangeYSpeed", 0.0f, SendMessageOptions.RequireReceiver);
    }

    public void doAction(string _actionName)
    {
        BroadcastMessage("DoAction", _actionName);
    }
    
    public void flip()
    {
        SetVar(TussleConstants.FighterVariableNames.FACING_DIRECTION, -1 * getBattleObject().GetIntVar(TussleConstants.FighterVariableNames.FACING_DIRECTION));
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
            getBattleObject().SendMessage("flip");
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
            getBattleObject().SendMessage("flip");
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
            //Asks the hitbox lock to put itself in the given list. This is odd, but since the lock is responsible for removing itself, it's easier this way
            hitbox.hitbox_lock.PutInList(hitbox_locks);
            //Starts the timer to remove this lock from the list
            StartCoroutine(RemoveLock(hitbox.hitbox_lock));
            return true;
        }
    }

    public void GetHit(Hitbox hitbox)
    {
        if (LockHitbox(hitbox)) //If the hitbox is not already locked to us
        {
            //Gah! A non-initialization getcomponent! Kill it with fire!
            //...as soon as I figure out HOW
            AbstractFighter otherFighter = hitbox.owner.GetComponent<AbstractFighter>();
            if (otherFighter != null)
            {
                hitTagged = otherFighter;
                if (hitTagCoroutine != null) StopCoroutine(hitTagCoroutine);
                hitTagCoroutine = RemoveTag();
                StartCoroutine(hitTagCoroutine);
            }

            float weight_constant = 1.4f;
            float flat_constant = 5.0f;

            float percent_portion = (damage_percent / 10.0f) + ((damage_percent * hitbox.damage) / 20.0f);
            float weight_portion = 200.0f / (GetFloatVar(TussleConstants.FighterAttributes.WEIGHT) * hitbox.weight_influence + 100);
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
        getBattleObject().BroadcastMessage("ChangeXSpeed",trajectory_vec.x);
        getBattleObject().BroadcastMessage("ChangeYSpeed", trajectory_vec.y);
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
            //DoHitStun(hitstun_frames * Settings.current_settings.hitstun_ratio, _trajectory);
            actionHandler.CurrentAction.SetVar("tech_cooldown", Mathf.RoundToInt(_total_kb * _hitstunMultiplier));
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

    public bool CheckSmash(string key)
    {
        if (key == "ForwardSmash")
        {
            if (GetIntVar(TussleConstants.FighterVariableNames.FACING_DIRECTION) == 1) key = "RightSmash";
            else key = "LeftSmash";
        }
        if (key == "BackwardSmash")
        {
            if (GetIntVar(TussleConstants.FighterVariableNames.FACING_DIRECTION) == 1) key = "LeftSmash";
            else key = "RightSmash";
        }
        return inputBuffer.KeyBuffered(key);
    }

    /////////////////////////////////////////////////////////////////////////////////////////
    //                                    TRIGGERS                                         //
    /////////////////////////////////////////////////////////////////////////////////////////
    void OnTriggerEnter(Collider other)
    {
        if (!other.transform.IsChildOf(this.transform))
        {
            contacted_colliders.Add(other);
            getBattleObject().PrintDebug(this, 3, "Entered Collider " + other);
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (!other.transform.IsChildOf(this.transform))
        {
            if (contacted_colliders.Contains(other))
                contacted_colliders.Remove(other);
            getBattleObject().PrintDebug(this, 3, "Left Collider " + other);
        }

        
        if (other.tag == "Killbox" && !other.bounds.Contains(transform.position))
        {
            GameObject deathBurst = ObjectPooler.current_pooler.GetPooledObject("DeathBurst");
            deathBurst.transform.position = transform.position;
            //TODO change color to that of the player that kill
            deathBurst.SetActive(true);

            Color deathCol = Settings.current_settings.player_colors[GetIntVar(TussleConstants.FighterVariableNames.PLAYER_NUM)];
            if (hitTagged != null) deathCol = Settings.current_settings.player_colors[hitTagged.GetIntVar(TussleConstants.FighterVariableNames.PLAYER_NUM)];
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
        yield return new WaitForSeconds(5);
        hitTagged = null;
        Debug.Log("Untagging");
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

    /// <summary>
    /// Recharge everything that happens on a "Rest", restoring jumps, airdodges, etc.
    /// </summary>
    void Rest()
    {
        SetVar(TussleConstants.FighterVariableNames.JUMPS_REMAINING, GetIntVar(TussleConstants.FighterAttributes.MAX_JUMPS));
        SetVar(TussleConstants.FighterVariableNames.AIR_DODGES_REMAINING, 1); //TODO change this based on settings
    }

    public void SetPlayerNum(int playernum)
    {
        SetVar(TussleConstants.FighterVariableNames.PLAYER_NUM, playernum);
    }
}