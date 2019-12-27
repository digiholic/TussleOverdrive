using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hitbox : MonoBehaviour {
    public BattleObject owner;

    public float damage = 0.0f;
    public float base_knockback = 0.0f;
    public float knockback_growth = 0.0f;
    public int trajectory = 0;
    public float weight_influence = 1.0f;
    public string lock_name = "";

    public Rect hitboxRect;
    
    public HitboxLock hitbox_lock;
    private Collider col;
    private bool active = false;
    private int _life = -1; //If Life is -1. last until deactivated manually
    private MeshRenderer mesh;

	// Use this for initialization
	void Awake () {
        col = GetComponent<Collider>();
        mesh = GetComponent<MeshRenderer>();
	}
	
    void Start()
    {
        if (BattleController.current_battle != null)
        {
            BattleController.current_battle.RegisterHitbox(this);
        }
    }


    // Update is called once per frame
    public void StepFrame () {
        if (active)
        {
            //Debug.Log("Checking for hitbox connections");
            Collider[] cols = Physics.OverlapBox(col.bounds.center, col.bounds.extents, col.transform.rotation, LayerMask.GetMask("Hurtbox"));
            foreach (Collider c in cols)
            {
                //Ignore hurtboxes and hitboxes from the same source
                if (!(transform.IsChildOf(c.transform.parent)))
                {
                    c.SendMessage("onHit", this);
                }
            }

            if (_life > 0)
                _life--;
            if (_life == 0)
                Deactivate();
        }
	}

    public void LoadValuesFromDict(Dictionary<string,string> dict)
    {
        Vector2 size = new Vector2(int.Parse(dict[Hitbox.WIDTH]),int.Parse(dict[Hitbox.HEIGHT]));
        hitboxRect.size = size;
        Vector2 center = new Vector2(int.Parse(dict[Hitbox.CENTER_X]),int.Parse(dict[Hitbox.CENTER_Y]));
        hitboxRect.center = center;
        
        //The all-important lock name
        if (dict.ContainsKey(Hitbox.LOCK_GROUP))
            lock_name = dict[Hitbox.LOCK_GROUP];
        
        //Hitbox stats
        if (dict.ContainsKey(Hitbox.DAMAGE))
            damage           = float.Parse(dict[Hitbox.DAMAGE]);
        if (dict.ContainsKey(Hitbox.BASE_KNOCKBACK))
            base_knockback = float.Parse(dict[Hitbox.BASE_KNOCKBACK]);
        if (dict.ContainsKey(Hitbox.KNOCKBACK_GROWTH))
            knockback_growth = float.Parse(dict[Hitbox.KNOCKBACK_GROWTH]);
        if (dict.ContainsKey(Hitbox.ANGLE))
            trajectory = int.Parse(dict[Hitbox.ANGLE]);

        if (owner != null)
            SizeToOwner(owner);
    }

    public void SizeToOwner(BattleObject obj)
    {
        float scale = obj.GetFloatVar(TussleConstants.SpriteVariableNames.PIXELS_PER_UNIT);
        //float scale = 50;
        Vector3 positionFromCenter = new Vector3(hitboxRect.center.x / scale, hitboxRect.center.y / scale, -0.1f);
        transform.localPosition = positionFromCenter;
        transform.localScale = new Vector3(hitboxRect.width / scale, hitboxRect.height / scale, 1.0f);
    }

    public void Activate(int life = -1)
    {
        _life = life;
        active = true;
        //if (FindObjectOfType<Settings>().display_hitboxes)
        //{
            mesh.enabled = true;
        //}
        
    }

    public void Deactivate()
    {
        if (active)
        {
            active = false;
            if (mesh.enabled)
                mesh.enabled = false;
            //hitbox_lock.Destroy();
        }
    }

    public void UnlockAll() //Allows anything to be hit by this hitbox again, even if it's locked
    {
        hitbox_lock.Destroy();
    }

    /// <summary>
    /// For debugging purposes, here's a better printout of a hitbox
    /// </summary>
    /// <returns></returns>
    public override string ToString()
    {
        string retString = "Hitbox object" + "\n";
        retString += "Center: " + hitboxRect.center.x + "," + hitboxRect.center.y + "\n";
        retString += "Size: " + hitboxRect.width + "," + hitboxRect.height + "\n";
        retString += "Damage: " + damage + "\n";
        retString += "Base KB: " + base_knockback + "\n";
        retString += "Knockback_growth: " + knockback_growth + "\n";
        retString += "Trajectory: " + trajectory + "\n";
        return retString;
    }

    void OnDestroy()
    {
        if (BattleController.current_battle != null)
            BattleController.current_battle.UnregisterHitbox(this);
    }

    public static string HITBOX_NAME = "hitboxName";
    public static string LOCK_GROUP = "lockGroup";
    public static string CENTER_X = "centerX";
    public static string CENTER_Y = "centerY";
    public static string WIDTH = "width";
    public static string HEIGHT = "height";
    public static string DAMAGE = "damage";
    public static string ANGLE = "angle";
    public static string BASE_KNOCKBACK = "baseKnockback";
    public static string KNOCKBACK_GROWTH = "knockbackGrowth";
    public static string CHARGE_DAMAGE = "chargeDamage";
    public static string CHARGE_BASE_KNOCKBACK = "chargeBaseKnockback";
    public static string CHARGE_KNOCKBACK_GROWTH = "chargeKnockbackGrowth";
    public static string SHIELD_DAMAGE = "shieldDamage";
    public static string HITSTUN_SCALING = "hitstunScaling";
    public static string SHIELDSTUN_SCALING = "shieldstunScaling";
    public static string SAKURAI_THRESHOLD = "sakuraiThreshold";
}

public class HitboxLock
{
    public string name = "";
    public List<List<HitboxLock>> locked_lists; //The hitbox lock needs to know who locked it so it can remove itself when it's no longer needed

    public HitboxLock(string _name)
    {
        name = _name;
        locked_lists = new List<List<HitboxLock>>();
    }

    /// <summary>
    /// On Destroy; remove itself from everything that it's locked into and unset its lock.
    /// </summary>
    public void Destroy()
    {
        foreach(List<HitboxLock> alist in locked_lists)
            alist.Remove(this);
        locked_lists.Clear();  //So this gets garbage collected, it can't have any more references
    }

    public void PutInList(List<HitboxLock> _list)
    {
        _list.Add(this);
        if (!locked_lists.Contains(_list)) //Lists should never get added in twice, but just to be sure
        {
            locked_lists.Add(_list);
        }
    }
}