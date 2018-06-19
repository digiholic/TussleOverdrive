using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Rewired;


public class ActionRow : MonoBehaviour {
    public string action_string;
    public InputActionType type;
    public AxisRange axisRange;
    public InputAction action;

    public List<ButtonEntry> button_rows = new List<ButtonEntry>();

	// Use this for initialization
	void Start () {
        action = ReInput.mapping.GetAction(action_string);
        foreach (ButtonEntry button_row in button_rows)
        {
            button_row.row = this;
        }
        UpdateText();
    }
	
	// Update is called once per frame
	void Update () {
	    	
	}

    void UpdateText()
    {
        int index = 0;
        BroadcastMessage("ChangeLabelText", "---");

        // Find the first ActionElementMap that maps to this Action and is compatible with this field type
        if (ControlSetter.current_setter != null)
        {
            //Debug.Log(ControlSetter.current_setter.controller);
            //Debug.Log(ControlSetter.current_setter.controllerMap);
            foreach (ActionElementMap actionElementMap in ControlSetter.current_setter.controllerMap.ElementMapsWithAction(action.id))
            {
                if (index >= button_rows.Count) break; //Escape the loop if we've set too many buttons already
                if (actionElementMap.ShowInField(axisRange))
                {
                    button_rows[index].ChangeLabelText(actionElementMap.elementIdentifierName);
                    button_rows[index].actionElementMapToReplaceId = actionElementMap.id;
                    index++;
                }
            }
        }
    }
}