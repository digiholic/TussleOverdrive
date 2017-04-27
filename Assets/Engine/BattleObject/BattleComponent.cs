using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleComponent : MonoBehaviour {
    protected BattleObject battleObject;

	// Use this for initialization
	void Awake () {
        battleObject = GetComponent<BattleObject>();
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
