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

                slider.eventReceiver = null;
                slider.sliderValue = (float)LegacyEditorData.instance.currentFrame / maxFrame;
                slider.eventReceiver = gameObject;
            }
        }
    }

    void OnSliderChange(float val)
    {
        ChangeCurrentFrame legacyAction = ScriptableObject.CreateInstance<ChangeCurrentFrame>();
        legacyAction.init(Mathf.FloorToInt(Mathf.Lerp(0, slider.numberOfSteps - 1, slider.sliderValue)), false, false);
        LegacyEditorData.instance.DoAction(legacyAction);
    }
}
