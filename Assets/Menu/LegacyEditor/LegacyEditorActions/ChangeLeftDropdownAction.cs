using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChangeLeftDropdownAction : LegacyEditorAction
{
    private string previousSelection = null;
    private string previousRightSelection = null;
    public string nextSelection;

    public void init(string selection)
    {
        nextSelection = selection;
    }

    public override void execute()
    {
        previousSelection = LegacyEditorData.instance.leftDropdown;
        previousRightSelection = LegacyEditorData.instance.rightDropdown;
        LegacyEditorData.instance.leftDropdown = nextSelection;
        LegacyEditorData.instance.rightDropdown = LegacyEditorConstants.RightDropdownOptionsDict[nextSelection][0]; //Set the right dropdown to the first option
    }

    public override void undo()
    {
        LegacyEditorData.instance.leftDropdown = previousSelection;
        LegacyEditorData.instance.rightDropdown = previousRightSelection;
    }
}
