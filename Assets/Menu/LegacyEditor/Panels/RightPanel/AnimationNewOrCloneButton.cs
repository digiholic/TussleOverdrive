using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationNewOrCloneButton : LegacyEditorWidget
{
    private UIButton button;
    private UILabel label;

    private void Awake()
    {
        button = GetComponent<UIButton>();
        label = GetComponentInChildren<UILabel>();
    }

    void OnAnimationChanged(AnimationDefinition def)
    {
        if (LegacyEditorData.CurrentAnimationIsNew())
        {
            label.text = "Create Animation";
        }
        else
        {
            label.text = "Duplicate Animation";
        }
    }

    public void OnAction()
    {
        CreateNewAnimation action = ScriptableObject.CreateInstance<CreateNewAnimation>();
        action.init(LegacyEditorData.instance.currentAnimation);
        LegacyEditorData.instance.DoAction(action);
    }

    public override void RegisterListeners()
    {
        editor.CurrentAnimationChangedEvent += OnAnimationChanged;
    }

    public override void UnregisterListeners()
    {
        editor.CurrentAnimationChangedEvent -= OnAnimationChanged;
    }
}
