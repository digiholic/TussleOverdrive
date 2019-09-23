using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActionSelectionButton : MonoBehaviour {
    public UILabel label;
    public UIButton button;

    public DynamicAction action;
	// Use this for initialization
	void Awake () {
        label = GetComponentInChildren<UILabel>();
        button = GetComponent<UIButton>();
        SetColor(null);
    }
	
	// Update is called once per frame
	void SetColor(DynamicAction act) {
        if (act != null && act == action)
        {
            label.color = new Color(1, 1, 1, 1);
            button.defaultColor = new Color(1, 1, 1, 1);
        }
        else
        {
            label.color = new Color(1, 1, 1, 0.5f);
            button.defaultColor = new Color(1, 1, 1, 0.5f);
        }
    }

    public void SetAction(DynamicAction actionToSet)
    {
        action = actionToSet;
        label.text = actionToSet.name;
        ModifyLegacyEditorDataAction legacyAction = ScriptableObject.CreateInstance<ModifyLegacyEditorDataAction>();
        legacyAction.init("currentAction", actionToSet);
        legacyAction.enableDeselect(DynamicAction.NullAction);
        legacyAction.addAdjacentProperty("currentFrame", 0);
        legacyAction.addAdjacentProperty("currentSubaction", null);

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
