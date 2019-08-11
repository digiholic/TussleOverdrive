using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationSelectionButton : MonoBehaviour
{
    public UILabel label;
    private UIButton button;
    public UIWidget widget;

    public AnimationDefinition animationDefinition;

    // Use this for initialization
    void Awake()
    {
        label = GetComponentInChildren<UILabel>();
        button = GetComponent<UIButton>();
        widget = GetComponent<UIWidget>();

        
    }

    void Start()
    {
        ChangedAnimation(LegacyEditorData.instance.currentAnimation);
    }

    void ChangedAnimation(AnimationDefinition anim)
    {
        if (anim != null && anim == animationDefinition)
        {
            button.defaultColor = new Color(1, 1, 1, 1);
        }
        else
        {
            button.defaultColor = new Color(1, 1, 1, 0.5f);
        }
    }

    public void SetAnimation(AnimationDefinition anim)
    {
        animationDefinition = anim;
        label.text = anim.AnimationName;
        ModifyLegacyEditorDataAction legacyAction = ScriptableObject.CreateInstance<ModifyLegacyEditorDataAction>();
        legacyAction.init("currentAnimation", anim);
        legacyAction.enableDeselect();

        GetComponent<OnClickSendAction>().action = legacyAction;
    }

    private void OnEnable()
    {
        LegacyEditorData.instance.CurrentAnimationChangedEvent += ChangedAnimation;
    }

    private void OnDisable()
    {
        LegacyEditorData.instance.CurrentAnimationChangedEvent -= ChangedAnimation;
    }
}
