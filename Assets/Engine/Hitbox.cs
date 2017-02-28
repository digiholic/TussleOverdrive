using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hitbox : MonoBehaviour {
    public float damage = 0.0f;
    public float base_knockback = 0.0f;
    public float knockback_growth = 0.0f;
    public int trajectory = 0;
    public float weight_influence = 1.0f;
    public string lock_name = "";

    public HitboxLock hitbox_lock;

    private Collider col;
    private bool active = false;
    private int _life = -1; //If Life is -1. last until deactivated
    private MeshRenderer mesh;

	// Use this for initialization
	void Start () {
        col = GetComponent<Collider>();
        mesh = GetComponent<MeshRenderer>();
	}
	
	// Update is called once per frame
	void Update () {
        if (active)
        {
            //Debug.Log("Checking for hitbox connections");
            Collider[] cols = Physics.OverlapBox(col.bounds.center, col.bounds.extents, col.transform.rotation, LayerMask.GetMask("Hurtbox"));
            foreach (Collider c in cols)
            {
                c.SendMessage("onHit", this);
                active = false;
            }

            if (_life > 0)
                _life--;
            if (_life == 0)
                Deactivate();
        }
	}

    public void LoadValuesFromDict(Dictionary<string,float> dict)
    {
        damage           = dict["damage"];
        base_knockback   = dict["base_knockback"];
        knockback_growth = dict["knockback_growth"];
        trajectory       = Mathf.FloorToInt(dict["trajectory"]);
    }

    public void Activate(int life = -1)
    {
        _life = life;
        active = true;
        mesh.enabled = true;
    }

    public void Deactivate()
    {
        active = false;
        mesh.enabled = false;
        hitbox_lock.Destroy();
    }

    public void UnlockAll() //Allows anything to be hit by this hitbox again, even if it's locked
    {
        hitbox_lock.Destroy();
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