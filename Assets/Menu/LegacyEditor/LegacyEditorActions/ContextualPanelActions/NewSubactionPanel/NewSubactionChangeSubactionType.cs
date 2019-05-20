using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NewSubactionChangeSubactionType : LegacyEditorAction
{
    private NewSubactionContextPanel executingPanelData = null;
    private SubactionType previousType = SubactionType.OTHER;
    private SubactionType nextType = SubactionType.OTHER;

    public void init(SubactionType subType)
    {
        nextType = subType;
    }

    public override void execute()
    {
        //This should only ever be run in the new subaction context panel. Otherwise, throw error
        if (ContextualPanelData.isOfType(typeof(NewSubactionContextPanel))) {
            executingPanelData = (NewSubactionContextPanel)LegacyEditorData.contextualPanel;
            previousType = executingPanelData.selectedType;
            executingPanelData.selectedType = nextType;
            executingPanelData.FireContextualPanelChange();
        } else {
            throw new System.Exception("Attempting to use NewSubactionChangeSubactionType from a Contextual Panel other than NewSubactionContextPanel");
        }
    }

    public override void undo()
    {
        executingPanelData.selectedType = previousType;
        executingPanelData.FireContextualPanelChange();
    }
}
