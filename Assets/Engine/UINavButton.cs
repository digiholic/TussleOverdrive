using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UINavButton : MonoBehaviour {

    public GameObject parentPanel;
    public GameObject targetPanel;

    
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    void NavigateTo()
    {
        parentPanel.SetActive(false);
        targetPanel.SetActive(true);
        targetPanel.SendMessage("GetSelected");
    }
}
