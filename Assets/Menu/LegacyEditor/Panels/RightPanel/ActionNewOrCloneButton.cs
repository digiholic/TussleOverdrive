using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActionNewOrCloneButton : LegacyEditorWidget {
    private UIButton button;
    private UILabel label;

    private void Awake()
    {
        button = GetComponent<UIButton>();
        label = GetComponentInChildren<UILabel>();
    }

    void OnActionChanged(DynamicAction action)
    {
        if (LegacyEditorData.CurrentActionIsNew())
        {
            label.text = "Create New Action";
        }
        else
        {
            label.text = "Duplicate Action";
        }
    }
    
    public void OnAction()
    {
        CreateNewAction action = ScriptableObject.CreateInstance<CreateNewAction>();
        action.init(LegacyEditorData.instance.currentAction);
        LegacyEditorData.instance.DoAction(action);
    }

    public override void RegisterListeners()
    {
        editor.CurrentActionChangedEvent += OnActionChanged;
    }

    public override void UnregisterListeners()
    {
        editor.CurrentActionChangedEvent -= OnActionChanged;
    }
}
