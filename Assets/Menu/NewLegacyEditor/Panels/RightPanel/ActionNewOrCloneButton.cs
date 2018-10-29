using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActionNewOrCloneButton : MonoBehaviour {
    private UIButton button;
    private UILabel label;

    private void Awake()
    {
        button = GetComponent<UIButton>();
        label = GetComponentInChildren<UILabel>();
    }

    void OnModelChanged()
    {
        if (LegacyEditorData.instance.currentActionDirty)
        {
            DynamicAction action = LegacyEditorData.instance.currentAction;
            if (LegacyEditorData.CurrentActionIsNew())
            {
                label.text = "Create New Action";
            } else
            {
                label.text = "Duplicate Action";
            }
        }
    }

    void OnAction()
    {
        CreateNewAction action = ScriptableObject.CreateInstance<CreateNewAction>();
        action.init(LegacyEditorData.instance.currentAction);
        LegacyEditorData.instance.DoAction(action);
    }
}
