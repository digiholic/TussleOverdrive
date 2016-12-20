using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class launch : MonoBehaviour {
    public Transform target = null;
    public float Speed = 10.0f;

    private int life;
    private Rigidbody rigidbody = null;
	// Use this for initialization
	void Start () {
        Debug.Log(transform.position);
        Debug.Log(target);
        transform.LookAt(target);
        rigidbody = GetComponent<Rigidbody>();
        rigidbody.velocity = transform.forward * Speed;
        life = 20;
    }
	
	// Update is called once per frame
	void Update () {
        life -= 1;
        if (life == 0)
        {
            Destroy(this.gameObject);
        }
    }
}
