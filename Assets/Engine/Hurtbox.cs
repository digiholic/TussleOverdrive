using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hurtbox : MonoBehaviour {

    public BattleObject owner;
    private MeshRenderer meshrenderer;

	// Use this for initialization
	void Start () {
        if (owner == null) owner = transform.root.GetComponent<BattleObject>();
        meshrenderer = GetComponent<MeshRenderer>();
        meshrenderer.enabled = Settings.current_settings.display_hurtboxes;
    }
	
	// Update is called once per frame
	void Update () {
		
	}

    void onHit(Hitbox hitbox)
    {
        //Debug.Log("Hurtbox has been hit");
        owner.SendMessage("GetHit",hitbox);
    }
}
