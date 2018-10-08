using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChangeCategoryDropdownAction : LegacyEditorAction
{
    private string previousSelection = null;
    private SubactionData previousSubaction = null;
    public string nextSelection;

    public void init(string selection)
    {
        nextSelection = selection;
    }

    public override void execute()
    {
        previousSelection = LegacyEditorData.instance.subactionCategory;
        previousSubaction = LegacyEditorData.instance.currentSubaction;
        LegacyEditorData.instance.subactionCategory = nextSelection;
        LegacyEditorData.instance.currentSubaction = null;
    }

    public override void undo()
    {
        LegacyEditorData.instance.subactionCategory = previousSelection;
        LegacyEditorData.instance.currentSubaction = previousSubaction;
    }
}
