using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimelineFrameCount : MonoBehaviour {
    public UILabel text;

    void OnModelChanged()
    {
        if (LegacyEditorData.instance.currentActionDirty || LegacyEditorData.instance.currentFrameDirty)
        {
            //Since the action can't be null, it turns into an anymous action whenever there's nothing loaded. If it's name is empty, it's basically null
            if (LegacyEditorData.instance.currentAction.name == "")
            {
                text.text = "---";
            } else
            {
                text.text = LegacyEditorData.instance.currentFrame.ToString() + "/" + LegacyEditorData.instance.currentAction.length.ToString();
            }
        }
    }
}
