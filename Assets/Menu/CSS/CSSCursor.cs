using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CSSCursor : MonoBehaviour {
    private bool clicking = false;
    private GameObject hovering_object;
    private GameObject selected_object;

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
        if (other.tag == "CSSPortrait")
        {
            hovering_object = other.gameObject;
            //Send message selected
        }
    }

    void OnTriggerStay(Collider other)
    {
        if (hovering_object == null) //If we have the edge case of exiting an object while hovering another one, reset the hovering object
        {
            hovering_object = other.gameObject;
        }
        if (clicking)
        {
            if (selected_object != hovering_object)
            {
                Debug.Log("selected");
                selected_object = hovering_object;
                //Send message selected
            }
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.gameObject == hovering_object)
            hovering_object = null;
    }
}
