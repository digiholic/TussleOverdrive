using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActionSelectionButton : MonoBehaviour {
    private UILabel label;

    public DynamicAction action;
	// Use this for initialization
	void OnEnable () {
        label = GetComponentInChildren<UILabel>();
	}
	
	// Update is called once per frame
	void Update () {
		if (LegacyEditorData.instance.currentAction == action)
        {
            label.color = new Color(1, 1, 1, 1);
        }
        else
        {
            label.color = new Color(1, 1, 1, 0.5f);
        }
    }

    public void SetAction(DynamicAction actionToSet)
    {
        action = actionToSet;
        label.text = actionToSet.name;
        ChangeCurrentAction legacyAction = ScriptableObject.CreateInstance<ChangeCurrentAction>();
        legacyAction.init(actionToSet);
        GetComponent<OnClickSendAction>().action = legacyAction;
    }
}
