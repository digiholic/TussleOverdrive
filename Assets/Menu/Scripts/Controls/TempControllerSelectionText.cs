using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Rewired;
using TMPro;

public class TempControllerSelectionText : MonoBehaviour {
    private TextMeshProUGUI label;

    // Use this for initialization
    void Awake()
    {
        label = GetComponent<TextMeshProUGUI>();
    }

    void UpdateText()
    {
        label.text = ControlSetter.current_setter.temp_controller.name;
    }
}
