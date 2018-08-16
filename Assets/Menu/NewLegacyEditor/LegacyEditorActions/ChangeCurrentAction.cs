using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChangeCurrentAction : LegacyEditorAction
{
    private DynamicAction previousAction = null;
    private int previousFrame = 0;
    public DynamicAction nextAction;

    public void init(DynamicAction act)
    {
        nextAction = act;
    }

    public override void execute()
    {
        previousAction = LegacyEditorData.instance.currentAction;
        previousFrame = LegacyEditorData.instance.currentFrame;
        LegacyEditorData.instance.currentAction = nextAction;
        LegacyEditorData.instance.currentFrame = 0;
    }

    public override void undo()
    {
        LegacyEditorData.instance.currentAction = previousAction;
        LegacyEditorData.instance.currentFrame = previousFrame;
    }
}
