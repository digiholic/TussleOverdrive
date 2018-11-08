using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeleteSubactionData : LegacyEditorAction {
    private SubactionData subDataToRemove;

    private DynamicAction prevAction;
    private string prevCategory;

    public void init(SubactionData data)
    {
        subDataToRemove = data;
    }

    public override void execute()
    {
        prevAction = LegacyEditorData.instance.currentAction;
        prevCategory = LegacyEditorData.instance.subactionGroup;

        prevAction.subactionCategories.GetIfKeyExists(prevCategory).Remove(subDataToRemove);
        LegacyEditorData.instance.currentSubaction = null;
        LegacyEditorData.ChangedSubaction();
    }

    public override void undo()
    {
        prevAction.subactionCategories.GetIfKeyExists(prevCategory).Add(subDataToRemove);
        LegacyEditorData.instance.currentSubaction = subDataToRemove;
        LegacyEditorData.ChangedSubaction();
    }
}
