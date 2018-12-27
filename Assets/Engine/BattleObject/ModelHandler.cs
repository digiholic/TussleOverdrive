using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class ModelHandler : BattleComponent {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    void flip()
    {
        transform.Rotate(transform.rotation.x, 180, transform.rotation.z);
    }
}
