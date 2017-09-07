using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shield : MonoBehaviour {
    private Renderer rend;

    public float shield_integrity = 1.0f;
    public bool shield_enabled;

	// Use this for initialization
	void Start () {
        rend = GetComponent<Renderer>();
	}
	
	// Update is called once per frame
	void Update () {
        rend.enabled = shield_enabled;
        transform.localScale = new Vector3(shield_integrity * 2, shield_integrity * 2, shield_integrity * 2);
	}

    void EnableShield()
    {
        shield_enabled = true;
    }

    void DisableShield()
    {
        shield_enabled = false;
    }
}
