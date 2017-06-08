using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NameSetter : MonoBehaviour {
    private UILabel label;

    void Awake()
    {
        label = GetComponent<UILabel>();
    }

	void ActionChanged(string name)
    {
        label.text = ActionFileEditor.action_file.Get(name).name;
    }
}
