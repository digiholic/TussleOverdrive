using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShowPanelIfDropdown : MonoBehaviour {
    public string leftDropdownCond;
    public string rightDropdownCond;

    public bool debug;
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    void OnModelChanged()
    {
        bool leftVal = true;
        bool rightVal = true;
        //If the dropdown conditional is empty, we just keep the condition true
        if (!leftDropdownCond.Equals(""))
        {
            if (LegacyEditorData.instance.leftDropdown.Equals(leftDropdownCond))
            {
                if (debug) Debug.Log("Left Value Valid");
                leftVal = true;
            } else
            {
                if (debug) Debug.Log("Left Value Invalid");
                leftVal = false;
            }
        }

        //If the dropdown conditional is empty, we just keep the condition true
        if (!rightDropdownCond.Equals(""))
        {
            if (LegacyEditorData.instance.rightDropdown.Equals(rightDropdownCond))
            {
                if (debug) Debug.Log("Right Value Valid");
                rightVal = true;
            }
            else
            {
                if (debug) Debug.Log("Right Value Invalid");
                rightVal = false;
            }
        }

        //If both conditionals are true
        if (leftVal && rightVal)
        {
            LegacyEditorData.Unbanish(gameObject);
        } else
        {
            LegacyEditorData.Banish(gameObject);
        }
    }
}
