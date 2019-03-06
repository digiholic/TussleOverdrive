using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimelineSlider : MonoBehaviour
{
    private UISlider slider;

    private void Awake()
    {
        slider = GetComponent<UISlider>();
    }

    void OnModelChanged()
    {
        if (LegacyEditorData.instance.currentActionDirty || LegacyEditorData.instance.currentFrameDirty)
        {
            //Since the action can't be null, it turns into an anymous action whenever there's nothing loaded. If it's name is empty, it's basically null
            if (LegacyEditorData.instance.currentAction.name == "")
            {
                slider.numberOfSteps = 1;
            }
            else
            {
                int maxFrame = LegacyEditorData.instance.currentAction.length;
                slider.numberOfSteps = maxFrame+1;
                /* ^^
                EventDelegate.Remove(slider.onChange, OnSliderChange);
                //slider.eventReceiver = null; ^^
                slider.value = (float)LegacyEditorData.instance.currentFrame / maxFrame;
                //slider.eventReceiver = gameObject; ^^
                EventDelegate.Set(slider.onChange, OnSliderChange);
                */
            }
        }
    }

    public void OnSliderChange()
    {
        float val = UISlider.current.value;
        ChangeCurrentFrame legacyAction = ScriptableObject.CreateInstance<ChangeCurrentFrame>();
        legacyAction.init(Mathf.FloorToInt(Mathf.Lerp(0, slider.numberOfSteps - 1, slider.value)), false, false);
        LegacyEditorData.instance.DoAction(legacyAction);
    }
}
