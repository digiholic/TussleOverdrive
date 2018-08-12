using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChangeLeftDropdownAction : LegacyEditorAction
{
    private string previousSelection = null;
    public string nextSelection;

    public void init(string selection)
    {
        nextSelection = selection;
    }

    public override void execute()
    {
        previousSelection = LegacyEditorData.instance.leftDropdown;
        LegacyEditorData.instance.leftDropdown = nextSelection;
    }

    public override void undo()
    {
        LegacyEditorData.instance.leftDropdown = previousSelection;
    }
}
