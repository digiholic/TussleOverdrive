using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CreateHitboxEditPanel : MonoBehaviour
{
    public CreateHitboxEditContextPanel.HitboxEditorCategory showWhenCategory;
    public GameObject panelToShow;

    void OnContextualPanelChanged()
    {
        //Only execute if it's the right kind of contextual panel
        if (ContextualPanelData.isOfType(typeof(CreateHitboxEditContextPanel)))
        {
            CreateHitboxEditContextPanel panel = (CreateHitboxEditContextPanel)LegacyEditorData.contextualPanel;
            if (panel.selectedCategory == showWhenCategory)
            {
                NGUITools.SetActive(panelToShow, true);
            }
            else
            {
                NGUITools.SetActive(panelToShow, false);
            }
        }
    }
}
