using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActionSelectionButton : MonoBehaviour {
    private UILabel label;

    public bool selected;
    public DynamicAction action;
	// Use this for initialization
	void OnEnable () {
        label = GetComponentInChildren<UILabel>();
	}
	
	// Update is called once per frame
	void Update () {
		if (selected)
        {
            label.color = new Color(1, 1, 1, 1);
        }
        if (!selected)
        {
            label.color = new Color(1, 1, 1, 0.5f);
        }
    }

    void toggleSelected()
    {
        selected = !selected;
    }

    public void SetAction(DynamicAction actionToSet)
    {
        action = actionToSet;
        label.text = actionToSet.name;
    }
}
