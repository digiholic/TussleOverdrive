using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class InputRecorder : MonoBehaviour {

    // Use this for initialization
	void Start () {

    }
	
	// Update is called once per frame
	void Update () {
	    	
	}

    public float GetControllerAxis(int player_num, string axisName)
    {
        return Input.GetAxisRaw(player_num + "_" + axisName);
    }

    public bool GetControllerButton(int player_num, string buttonName)
    {
        return Input.GetButton(player_num + "_" + buttonName);
    }

    public bool GetControllerButtonDown(int player_num, string buttonName)
    {
        return Input.GetButtonDown(player_num + "_" + buttonName);
    }

    public bool GetControllerButtonUp(int player_num, string buttonName)
    {
        return Input.GetButtonUp(player_num + "_" + buttonName);
    }
}
