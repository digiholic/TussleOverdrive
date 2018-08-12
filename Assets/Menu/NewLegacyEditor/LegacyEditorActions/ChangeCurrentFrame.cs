using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChangeCurrentFrame : LegacyEditorAction
{
    private int previousFrame = 0;
    private bool isRelative, isLast;
    public int nextFrame;


    public void init(int frame, bool relative, bool last)
    {
        nextFrame = frame;
        isRelative = relative;
        isLast = last;
    }

    public override void execute()
    {
        int currentFrame = LegacyEditorData.instance.currentFrame;
        int last = LegacyEditorData.instance.currentAction.length;

        int targetFrame = nextFrame;
        if (isRelative)
        {
            targetFrame = currentFrame + nextFrame;
        }
        else if (isLast)
        {
            targetFrame = last;
        }

        if (targetFrame < 0) targetFrame = 0;
        if (targetFrame > last) targetFrame = last;
        
        previousFrame = LegacyEditorData.instance.currentFrame;
        LegacyEditorData.instance.currentFrame = targetFrame;
    }

    public override void undo()
    {
        LegacyEditorData.instance.currentFrame = previousFrame;
    }
}
