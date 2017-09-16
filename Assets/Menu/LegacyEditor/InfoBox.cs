using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InfoBox : MonoBehaviour {
    public UILabel label;

    [TextArea(6, 10)]
    public string display_text;

    void Start()
    {
        //label.text = display_text;
    }

    public void SetText(string text)
    {
        Debug.Log(text);
        label.text = text;
    }
}
