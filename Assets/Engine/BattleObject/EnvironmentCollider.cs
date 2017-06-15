using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class EnvironmentCollider : BattleComponent {
    public Vector3 center_offset;
    public float radius;
    public float height;
    
    // Use this for initialization
	void Start () {

    }
	
	// Update is called once per frame
	void Update () {
		
	}

    void OnCollisionEnter(Collision col)
    {
        if (col.gameObject.layer == LayerMask.NameToLayer("Terrain"))
        {
            Debug.Log("Hit terrain");
        }
    }
}
