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

    public int centerx = 0;
    public int centery = 0;
    public int width = 0;
    public int height = 0;

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
                if (c.transform.parent != transform.parent)
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

    public void LoadValuesFromDict(AbstractFighter owner, Dictionary<string,string> dict)
    {
        //Set the centerpoint
        if (dict.ContainsKey("center"))
        {
            string[] center = dict["center"].Split(',');
            centerx = int.Parse(center[0]);
            centery = int.Parse(center[1]);
        }
        //Set the size
        if (dict.ContainsKey("size"))
        {
            string[] size = dict["size"].Split(',');
            width = int.Parse(size[0]);
            height = int.Parse(size[1]);
        }

        float scale = owner.GetComponent<SpriteHandler>().pixelsPerUnit;
        //float scale = 50;
        transform.localPosition = new Vector3(centerx / scale, centery / scale, 0.0f);
        transform.localScale = new Vector3(width / scale, height / scale, 1.0f);

        //The all-important lock name
        if (dict.ContainsKey("lock_name"))
            lock_name = dict["lock_name"];
        
        //Hitbox stats
        if (dict.ContainsKey("damage"))
            damage           = float.Parse(dict["damage"]);
        if (dict.ContainsKey("base_knockback"))
            base_knockback = float.Parse(dict["base_knockback"]);
        if (dict.ContainsKey("knockback_growth"))
            knockback_growth = float.Parse(dict["knockback_growth"]);
        if (dict.ContainsKey("trajectory"))
            trajectory = int.Parse(dict["trajectory"]);
        
    }

    public void Activate(int life = -1)
    {
        _life = life;
        active = true;
        if (FindObjectOfType<Settings>().display_hitboxes)
        {
            mesh.enabled = true;
        }
        
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
        retString += "Center: " + centerx + "," + centery + "\n";
        retString += "Size: " + width + "," + height + "\n";
        retString += "Damage: " + damage + "\n";
        retString += "Base KB: " + base_knockback + "\n";
        retString += "Knockback_growth: " + knockback_growth + "\n";
        retString += "Trajectory: " + trajectory + "\n";
        return retString;
    }

    void OnDestroy()
    {
        BattleController.current_battle.UnregisterHitbox(this);
    }
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
    /// On Destroy, remove itself from everything that it's locked into and unset its lock.
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