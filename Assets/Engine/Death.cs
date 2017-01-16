using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Death : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    void OnTriggerExit(Collider other)
    {
        if (other.tag == "Killbox")
        {
            GetComponent<Transform>().position = new Vector3(0, 10);
            //TODO send death signal, handle respawning in-object
            if (GetComponent<Rigidbody>() != null)
            {
                GetComponent<Rigidbody>().velocity = Vector3.zero;
            }
        }
    }
}
