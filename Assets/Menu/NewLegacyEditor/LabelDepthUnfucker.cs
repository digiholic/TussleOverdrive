using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// NGUI has this really frustrating bug where labels aren't drawn above clipped sprites
/// no matter what the depth is, until their depth is changed and it sort of "wakes up" the
/// renderer. This class will unfuck the renderer up by shifting it forward whenever you need to.
/// </summary>
public class LabelDepthUnfucker : MonoBehaviour
{
    private UILabel label;
    private UISprite sprite;
    private bool isUnfucked = false;

    // Start is called before the first frame update
    void Awake()
    {
        label = GetComponent<UILabel>();
        sprite = GetComponent<UISprite>();
    }

    /// <summary>
    /// Change the label's depth up by one. This should let the
    /// render queue know something has changed and fix the layering on it.
    /// </summary>
    public void UnfuckLabelDepth()
    {
        Invoke("incrementLabelDepth", 0.1f);
    }

    public void RefuckLabelDepth()
    {
        decrementLabelDepth();
    }

    private void incrementLabelDepth()
    {
        //Don't unfuck twice
        if (!isUnfucked)
        {
            if (label != null) label.depth = label.depth + 1;
            if (sprite != null) sprite.depth = sprite.depth + 1;
            isUnfucked = true;
        }
    }

    private void decrementLabelDepth()
    {
        //Don't unfuck twice
        if (isUnfucked)
        {
            if (label != null) label.depth = label.depth - 1;
            if (sprite != null) sprite.depth = sprite.depth - 1;
            isUnfucked = false;
        }
    }
}
