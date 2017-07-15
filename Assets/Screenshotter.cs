using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Screenshotter : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        if (Input.GetKeyDown(KeyCode.LeftBracket))
        {
            ScreenCapture.CaptureScreenshot("screenshot.png");
            Debug.Log("Screenshot captured");
        }
            
    }
}
