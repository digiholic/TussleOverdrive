using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sandbag : MonoBehaviour {

    private Rigidbody rb;
    private bool launching = false;

	// Use this for initialization
	void Start () {
        rb = GetComponent<Rigidbody>();
	}
	
	// Update is called once per frame
	void Update () {
        if (launching && rb.velocity.magnitude > 1.0f)
        {
            Quaternion newRotation = Quaternion.FromToRotation(transform.up, rb.velocity);
            transform.rotation = Quaternion.Slerp(transform.rotation, newRotation, Time.deltaTime * 8);
        }
    }

    void OnCollisionEnter(Collision collision)
    {
        if (collision.collider.gameObject.layer == LayerMask.NameToLayer("Terrain"))
        {
            //Debug.Log("Landed");
            launching = false;
        }
    }

    void getLaunched(Hitbox hitbox)
    {
        Vector3 launchVector = new Vector3(hitbox.base_knockback, 0, 0);
        launchVector = Quaternion.Euler(0, 0, hitbox.trajectory) * launchVector;
        //Debug.Log(launchVector);
        rb.AddForce(launchVector);
        //GetComponent<CharacterController>().Move(launchVector);
        launching = true;
    }
}
