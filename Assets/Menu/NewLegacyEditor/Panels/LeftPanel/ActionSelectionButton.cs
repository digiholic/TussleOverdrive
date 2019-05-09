using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActionSelectionButton : MonoBehaviour {
    public UILabel label;

    public DynamicAction action;
	// Use this for initialization
	void Awake () {
        label = GetComponentInChildren<UILabel>();
        SetColor(null);
    }
	
	// Update is called once per frame
	void SetColor(DynamicAction act) {
        if (act != null && act == action)
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

    private void OnEnable()
    {
        LegacyEditorData.instance.CurrentActionChangedEvent += SetColor;
    }

    private void OnDisable()
    {
        LegacyEditorData.instance.CurrentActionChangedEvent -= SetColor;
    }
}
