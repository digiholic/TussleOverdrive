using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Rewired;
using TMPro;

public class ButtonEntry : MonoBehaviour {
    private TextMeshProUGUI label;
    public ActionRow row;
    public int actionElementMapToReplaceId;


    // Use this for initialization
    void Awake() {
        label = GetComponent<TextMeshProUGUI>();
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
