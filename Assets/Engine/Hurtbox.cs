using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hurtbox : MonoBehaviour {

    private Transform fighter;
    
	// Use this for initialization
	void Start () {
        fighter = GetComponent<Collider>().transform.root;
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    void onHit(Hitbox hitbox)
    {
        //Debug.Log("Hurtbox has been hit");
        fighter.SendMessage("GetHit",hitbox);
    }
}
