using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HitboxContextChangeCategory : LegacyEditorAction
{
    private CreateHitboxEditContextPanel executingPanelData = null;
    private CreateHitboxEditContextPanel.HitboxEditorCategory previousCategory = CreateHitboxEditContextPanel.HitboxEditorCategory.PROPERTIES;
    private CreateHitboxEditContextPanel.HitboxEditorCategory nextCategory = CreateHitboxEditContextPanel.HitboxEditorCategory.PROPERTIES;

    public void init(CreateHitboxEditContextPanel.HitboxEditorCategory category)
    {
        nextCategory = category;
    }

    public override void execute()
    {
        //This should only ever be run in the new subaction context panel. Otherwise, throw error
        if (ContextualPanelData.isOfType(typeof(CreateHitboxEditContextPanel)))
        {
            executingPanelData = (CreateHitboxEditContextPanel)LegacyEditorData.contextualPanel;
            previousCategory = executingPanelData.selectedCategory;
            executingPanelData.selectedCategory = nextCategory;
            executingPanelData.FireContextualPanelChange();
        }
        else
        {
            throw new System.Exception("Attempting to use NewSubactionChangeSubactionType from a Contextual Panel other than NewSubactionContextPanel");
        }
    }

    public override void undo()
    {
        executingPanelData.selectedCategory = previousCategory;
        executingPanelData.FireContextualPanelChange();
    }
}
