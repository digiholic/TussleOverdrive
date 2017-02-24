using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlatformPhase : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    void OnTriggerEnter(Collider other)
    {
        if (other.tag == "PassThrough")
        {
            Physics.IgnoreCollision(GetComponent<CharacterController>(), other.transform.parent.GetComponent<Collider>(), true);
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.tag == "PassThrough")
        {
            Physics.IgnoreCollision(GetComponent<CharacterController>(), other.transform.parent.GetComponent<Collider>(), false);
        }
    }

}
