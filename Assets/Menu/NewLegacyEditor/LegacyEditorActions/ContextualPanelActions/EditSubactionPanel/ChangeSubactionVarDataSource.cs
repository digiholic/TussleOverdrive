using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChangeSubactionVarDataSource : LegacyEditorAction {
    private SubactionVarData varDataToEdit;
    private SubactionSource sourceToSet;
    private SubactionSource previousSource;

    public void init(SubactionVarData varData, SubactionSource source)
    {
        varDataToEdit = varData;
        sourceToSet = source;
    }

    public override void execute()
    {
        previousSource = varDataToEdit.source;
        varDataToEdit.source = sourceToSet;
    }

    public override void undo()
    {
        varDataToEdit.source = previousSource;
    }
}
