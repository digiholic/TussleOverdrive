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
        DynamicAction actionToSet = nextAction;
        //If we're re-selecting the current action, unselect it instead (by creating an empty action that can be edited and saved later)
        previousAction = LegacyEditorData.instance.currentAction;
        if (previousAction == nextAction)
        {
            actionToSet = new DynamicAction("");
        }

        previousFrame = LegacyEditorData.instance.currentFrame;
        LegacyEditorData.instance.currentAction = actionToSet;
        LegacyEditorData.instance.currentFrame = 0;
    }

    public override void undo()
    {
        LegacyEditorData.instance.currentAction = previousAction;
        LegacyEditorData.instance.currentFrame = previousFrame;
    }
}
