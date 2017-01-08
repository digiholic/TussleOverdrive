using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hitbox : MonoBehaviour {
    public float damage = 0.0f;
    public float base_knockback = 0.0f;
    public float knockback_growth = 0.0f;
    public int trajectory = 0;

    private Collider col;
    private bool active = false;
    private int _life = -1; //If Life is -1. last until deactivated

	// Use this for initialization
	void Start () {
        col = GetComponent<Collider>();
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

    public void Activate(int life = -1)
    {
        _life = life;
        active = true;
    }

    public void Deactivate()
    {
        active = false;
    }
}
