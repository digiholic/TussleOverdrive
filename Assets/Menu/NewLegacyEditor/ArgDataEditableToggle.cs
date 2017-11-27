using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArgDataEditableToggle : MonoBehaviour {
    public Collider clickCollider;
    public UISprite background;

	// Use this for initialization
	void Awake () {
        clickCollider = GetComponent<Collider>();
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    void SetEditable(bool editable)
    {
        if (editable)
        {
            clickCollider.enabled = true;
            background.enabled = true;
        }
        else
        {
            clickCollider.enabled = false;
            background.enabled = false;
        }
    }
}
