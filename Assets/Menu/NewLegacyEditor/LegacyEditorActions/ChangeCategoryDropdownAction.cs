using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChangeCategoryDropdownAction : LegacyEditorAction
{
    private string previousSelection = null;
    public string nextSelection;

    public void init(string selection)
    {
        nextSelection = selection;
    }

    public override void execute()
    {
        previousSelection = LegacyEditorData.instance.subactionCategory;
        LegacyEditorData.instance.subactionCategory = nextSelection;
    }

    public override void undo()
    {
        LegacyEditorData.instance.subactionCategory = previousSelection;
    }
}
