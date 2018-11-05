using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SubactionVarDataNameLabel : MonoBehaviour {
    public SubactionVarDataPanel panel;
    private UILabel label;

    // Use this for initialization
    void OnEnable()
    {
        label = GetComponent<UILabel>();
    }

    private void Start()
    {
        label.text = panel.varData.name;
    }
}
