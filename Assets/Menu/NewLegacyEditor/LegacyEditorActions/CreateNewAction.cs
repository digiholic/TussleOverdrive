using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CreateNewAction : LegacyEditorAction
{
    public DynamicAction actionToAdd = null;

    public void init(DynamicAction action)
    {
        actionToAdd = action;
    }
    public override void execute()
    {
        LegacyEditorData.instance.loadedActionFile.Add(actionToAdd, false);
        LegacyEditorData.ChangedActionFile();
    }

    public override void undo()
    {
        LegacyEditorData.instance.loadedActionFile.Delete(actionToAdd);
        LegacyEditorData.ChangedActionFile();
    }
}
