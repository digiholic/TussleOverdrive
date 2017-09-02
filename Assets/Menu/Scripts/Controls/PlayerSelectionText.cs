using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Rewired;

public class PlayerSelectionText : MonoBehaviour {
    private UILabel label;


    // Use this for initialization
    void Awake () {
        label = GetComponentInChildren<UILabel>();
    }

    // Update is called once per frame
    void Update () {
		
	}

    void UpdateText()
    {
        label.text = "Player: " + ControlSetter.current_setter.player.descriptiveName;
    }

    void IncrementValue()
    {
        ControlSetter.current_setter.ChangePlayer(1);
    }

    void DecrementValue()
    {
        ControlSetter.current_setter.ChangePlayer(-1);
    }
}
