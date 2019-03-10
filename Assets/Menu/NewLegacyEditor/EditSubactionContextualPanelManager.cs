using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EditSubactionContextualPanelManager : LegacyEditorWidget
{

    public EditSubactionContextPanel defaultSubactionContextPanel;
    public SubactionToContextPanelDict uniqueContextPanels;

    void OnSubactionChanged(SubactionData subaction)
    {
        if (subaction == null)
        {
            defaultSubactionContextPanel.DeactivatePanel();
        }
        else
        {
            if (uniqueContextPanels.ContainsKey(subaction.SubactionName))
            {
                if (LegacyEditorData.contextualPanel != null)
                {
                    LegacyEditorData.contextualPanel.DeactivatePanel();
                }
                uniqueContextPanels[subaction.SubactionName].ActivatePanel();
            }
            else
            {
                if (LegacyEditorData.contextualPanel != null)
                {
                    LegacyEditorData.contextualPanel.DeactivatePanel();
                }
                defaultSubactionContextPanel.ActivatePanel();
            }
        }
    }

    public override void RegisterListeners()
    {
        editor.CurrentSubactionChangedEvent += OnSubactionChanged;
    }

    public override void UnregisterListeners()
    {
        editor.CurrentSubactionChangedEvent -= OnSubactionChanged;
    }

}


/// <summary>
/// This derivation is necessary for the Serializable Dictionary to drawn in inspector
/// </summary>
[System.Serializable]
public class SubactionToContextPanelDict : SerializableDictionary<string, EditSubactionContextPanel>{}