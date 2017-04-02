using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuPanel : MonoBehaviour {
    public GameObject SelectedObject;

	// Use this for initialization
	void Start () {

	}
	
	// Update is called once per frame
	void Update () {
        if (NGUITools.GetActive(gameObject))
        {
            SelectedObject = UICamera.selectedObject;
        }
    }

    void GetSelected()
    {
        UICamera.selectedObject = SelectedObject;
        UICamera.selectedObject.SendMessage("OnHover", true);
    }
}
