using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CSSCursor : MonoBehaviour {
    private bool clicking = false;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        Vector3 temp = Input.mousePosition;
        temp.z = 0f;
        this.transform.position = Camera.main.ScreenToWorldPoint(temp);
        clicking = Input.GetMouseButtonDown(0);
    }

    void OnTriggerEnter(Collider other)
    {
        if (clicking)
        {
            if (other.tag == "CSSPortrait")
            {
                Debug.Log("Hovering");
            }
        }
    }
}
