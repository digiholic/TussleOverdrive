using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Rewired;
using TMPro;

public class ControllerSelectionText : MonoBehaviour {
    private TextMeshProUGUI label;

    // Use this for initialization
    void Start()
    {
        label = GetComponentInChildren<TextMeshProUGUI>();
    }

    void UpdateText()
    {
        label.text = "Controller: " + ControlSetter.current_setter.controller.name;
    }

    void ConfirmChangeController()
    {
        ControlSetter.current_setter.ConfirmChangeController();
    }

    void IncrementValue()
    {
        ControlSetter.current_setter.ChangeTempController(1);
        //ControlSetter.current_setter.ChangeController(1);
    }

    void DecrementValue()
    {
        ControlSetter.current_setter.ChangeTempController(-1);
        //ControlSetter.current_setter.ChangeController(-1);
    }
}
