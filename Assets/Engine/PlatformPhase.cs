using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlatformPhase : MonoBehaviour {
    public bool EnableDownPhase { get; set; } //Default to not allowing downward phasing
    public bool EnableUpPhase { get; set; } //Default to allowing upward phasing

    private CharacterController _charController;

	// Use this for initialization
	void Start () {
        _charController = GetComponent<CharacterController>();
        EnableDownPhase = false;
        EnableUpPhase = true;
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    void OnTriggerEnter(Collider other)
    {
        if (other.tag == "PassThrough" && EnableUpPhase)
        {
            Physics.IgnoreCollision(_charController, other.transform.parent.GetComponent<Collider>(), true);
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.tag == "PassThrough")
        {
            Physics.IgnoreCollision(_charController, other.transform.parent.GetComponent<Collider>(), false);
        }

        if (other.tag == "DropDown")
        {
            Physics.IgnoreCollision(_charController, other.transform.parent.GetComponent<Collider>(), false);
        }
    }

    void OnTriggerStay(Collider other)
    {
        if (other.tag == "DropDown" && EnableDownPhase)
        {
            Physics.IgnoreCollision(_charController, other.transform.parent.GetComponent<Collider>(), true);
        }
    }
}
