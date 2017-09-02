using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Rewired;

public class ControllerSelectionText : MonoBehaviour {
    private UILabel label;

    // Use this for initialization
    void Start()
    {
        label = GetComponentInChildren<UILabel>();
    }

    void UpdateText()
    {
        label.text = "Controller: " + ControlSetter.current_setter.controller.name;
    }

    void IncrementValue()
    {
        ControlSetter.current_setter.ChangeController(1);
    }

    void DecrementValue()
    {
        ControlSetter.current_setter.ChangeController(-1);
    }
}
