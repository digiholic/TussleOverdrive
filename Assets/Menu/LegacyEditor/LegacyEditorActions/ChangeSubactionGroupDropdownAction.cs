using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChangeSubactionGroupDropdownAction : LegacyEditorAction
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
        previousSelection = LegacyEditorData.instance.subactionGroup;
        previousSubaction = LegacyEditorData.instance.currentSubaction;
        LegacyEditorData.instance.subactionGroup = nextSelection;
        LegacyEditorData.instance.currentSubaction = null;
    }

    public override void undo()
    {
        LegacyEditorData.instance.subactionGroup = previousSelection;
        LegacyEditorData.instance.currentSubaction = previousSubaction;
    }
}
