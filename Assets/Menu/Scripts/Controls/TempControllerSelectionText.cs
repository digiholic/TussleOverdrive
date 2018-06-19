using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Rewired;

public class TempControllerSelectionText : MonoBehaviour {
    private UILabel label;

    // Use this for initialization
    void Awake()
    {
        label = GetComponent<UILabel>();
    }

    void UpdateText()
    {
        label.text = ControlSetter.current_setter.temp_controller.name;
    }
}
