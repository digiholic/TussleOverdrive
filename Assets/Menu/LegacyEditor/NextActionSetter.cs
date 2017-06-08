using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NextActionSetter : MonoBehaviour {

    private UILabel label;

    void Awake()
    {
        label = GetComponent<UILabel>();
    }

    void ActionChanged(string name)
    {
        label.text = ActionFileEditor.action_file.Get(name).exit_action;
    }
}