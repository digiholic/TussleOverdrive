using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Rewired;

public class ButtonEntry : MonoBehaviour {
    private UILabel label;
    public ActionRow row;
    public int actionElementMapToReplaceId;


    // Use this for initialization
    void Awake() {
        label = GetComponent<UILabel>();
	}

    public void ChangeLabelText(string newtext)
    {
        label.text = newtext;
    }

    void MapButton()
    {
        ControlSetter.current_setter.MapKey(row.action,row.axisRange,actionElementMapToReplaceId);
    }
}
