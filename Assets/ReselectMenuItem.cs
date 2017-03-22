using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ReselectMenuItem : MonoBehaviour {
    private GameObject last_selected_item;

	// Use this for initialization
	void Start () {
        UICamera.genericEventHandler = this.gameObject;
        last_selected_item = UICamera.selectedObject;
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void OnSelect(bool selected)
    {

        if (selected)
        {
            if (UICamera.selectedObject == this.gameObject)
            {
                UICamera.selectedObject = last_selected_item;
            }
            last_selected_item = UICamera.selectedObject;
        }
    }
}
