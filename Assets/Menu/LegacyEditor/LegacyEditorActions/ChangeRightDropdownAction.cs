using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChangeRightDropdownAction : LegacyEditorAction
{
    private string previousSelection = null;
    public string nextSelection;

    public void init(string selection)
    {
        nextSelection = selection;
    }

    public override void execute()
    {
        previousSelection = LegacyEditorData.instance.rightDropdown;
        LegacyEditorData.instance.rightDropdown = nextSelection;
    }

    public override void undo()
    {
        LegacyEditorData.instance.rightDropdown = previousSelection;
    }
}
