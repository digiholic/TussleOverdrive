using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class EnvironmentCollider : BattleComponent {
    public Vector3 center_offset;
    public float radius;
    public float height;
    
    private CharacterController char_controller;

	// Use this for initialization
	void Start () {
        char_controller = GetComponent<CharacterController>();
        if (char_controller == null) //If the character controller isn't already set, we need to create it
        {
            char_controller = gameObject.AddComponent<CharacterController>();
        }
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
