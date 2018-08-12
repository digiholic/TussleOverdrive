using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimelineFrameCount : MonoBehaviour {
    public UILabel text;

    void OnModelChanged()
    {
        if (LegacyEditorData.instance.currentActionDirty || LegacyEditorData.instance.currentFrameDirty)
        {
            text.text = LegacyEditorData.instance.currentFrame.ToString() + "/" + LegacyEditorData.instance.currentAction.length.ToString();
        }
    }
}
