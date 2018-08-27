using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeleteAction : LegacyEditorAction {
    public DynamicAction actionToDelete = null;
    private DynamicAction previousAction = null;
    private int previousFrame = 0;
    private int previousIndex = 0;

    public void init(DynamicAction action)
    {
        actionToDelete = action;
    }

    public override void execute()
    {
        //First we have to change to a null action, then we can delete the old one.
        previousAction = actionToDelete;
        previousFrame = LegacyEditorData.instance.currentFrame;
        LegacyEditorData.instance.currentAction = new DynamicAction(""); //Set to a null action
        LegacyEditorData.instance.currentFrame = 0;

        previousIndex = LegacyEditorData.instance.loadedActionFile.actions.IndexOf(actionToDelete);
        LegacyEditorData.instance.loadedActionFile.Delete(actionToDelete);
        LegacyEditorData.ChangedActionFile();
    }

    public override void undo()
    {
        LegacyEditorData.instance.currentAction = previousAction;
        LegacyEditorData.instance.currentFrame = previousFrame;
        LegacyEditorData.instance.loadedActionFile.actions.Insert(previousIndex, previousAction); //Add it back where it was
        LegacyEditorData.ChangedActionFile();
    }
}
