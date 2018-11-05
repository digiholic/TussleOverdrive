using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChangeSubactionVarDataInput : LegacyEditorAction
{
    private SubactionVarData varDataToEdit;
    private string dataToSet;
    private string previousData;

    public void init(SubactionVarData varData, string data)
    {
        varDataToEdit = varData;
        dataToSet = data;
    }

    public override void execute()
    {
        previousData = varDataToEdit.data;
        varDataToEdit.data = dataToSet;
    }

    public override void undo()
    {
        varDataToEdit.data = previousData;
    }
}
