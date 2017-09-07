using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hurtbox : MonoBehaviour {

    private Transform fighter;
    private MeshRenderer meshrenderer;

	// Use this for initialization
	void Start () {
        fighter = transform.root;
        meshrenderer = GetComponent<MeshRenderer>();
        meshrenderer.enabled = Settings.current_settings.display_hurtboxes;
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
