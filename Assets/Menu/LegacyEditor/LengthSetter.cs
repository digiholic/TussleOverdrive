using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LengthSetter : MonoBehaviour
{
    private UILabel label;

    void Awake()
    {
        label = GetComponent<UILabel>();
        Debug.Log(label);
    }

    void ActionChanged(string name)
    {
        label.text = ActionFileEditor.action_file.Get(name).length.ToString();
    }
}
