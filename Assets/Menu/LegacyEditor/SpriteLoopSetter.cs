using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpriteLoopSetter : MonoBehaviour {

    private UICheckbox label;

    void Awake()
    {
        label = GetComponent<UICheckbox>();
    }

    void ActionChanged(string name)
    {
        label.isChecked = ActionFileEditor.action_file.Get(name).loop;
    }
}
