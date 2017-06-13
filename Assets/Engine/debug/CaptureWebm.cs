using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CaptureWebm : MonoBehaviour {
    private bool recording = false;

    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        if (Input.GetKeyDown(KeyCode.P))
        {
            if (recording)
            {
                Camera.main.GetComponent<Moments.Recorder>().Save();
                recording = false;
                Debug.Log("Finished recording");
            }
            else
            {
                Camera.main.GetComponent<Moments.Recorder>().Record();
                recording = true;
                Debug.Log("Started recording");
            }
        }
    }
}