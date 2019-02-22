using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// NGUI has this really frustrating bug where labels aren't drawn above clipped sprites
/// no matter what the depth is, until their depth is changed and it sort of "wakes up" the
/// renderer. This class will unfuck the renderer up by shifting it forward whenever you need to.
/// </summary>
[RequireComponent(typeof(UILabel))]
public class LabelDepthUnfucker : MonoBehaviour
{
    private UILabel label;
    private bool isUnfucked = false;

    // Start is called before the first frame update
    void Awake()
    {
        label = GetComponent<UILabel>();
    }

    /// <summary>
    /// Change the label's depth up by one. This should let the
    /// render queue know something has changed and fix the layering on it.
    /// </summary>
    public void UnfuckLabelDepth()
    {
        Invoke("incrementLabelDepth", 0.1f);
    }

    private void incrementLabelDepth()
    {
        if (isUnfucked)
        {
            label.depth = label.depth - 1;
        }
        else
        {
            label.depth = label.depth + 1;
        }
        isUnfucked = !isUnfucked;
    }
}
