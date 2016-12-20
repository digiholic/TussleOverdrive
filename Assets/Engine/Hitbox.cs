using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hitbox : MonoBehaviour {
    public float damage = 0.0f;
    public float base_knockback = 0.0f;
    public float knockback_growth = 0.0f;
    public int trajectory = 0;

    private Collider col;
	// Use this for initialization
	void Start () {
        col = GetComponent<Collider>();
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void LaunchAttack()
    {
        Collider[] cols = Physics.OverlapBox(col.bounds.center, col.bounds.extents, col.transform.rotation, LayerMask.GetMask("Hurtbox"));

        foreach (Collider c in cols)
        {
            c.SendMessage("onHit",this);
        }
    }
}
