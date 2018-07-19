using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChangeCurrentAction : LegacyEditorAction
{
    private DynamicAction previousAction = null;
    public DynamicAction nextAction;

    public void init(DynamicAction act)
    {
        nextAction = act;
    }

    public override void execute()
    {
        previousAction = LegacyEditorData.instance.currentAction;
        LegacyEditorData.instance.currentAction = nextAction;
    }

    public override void undo()
    {
        LegacyEditorData.instance.currentAction = previousAction;
    }
}
