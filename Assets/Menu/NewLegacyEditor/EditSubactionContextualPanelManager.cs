using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EditSubactionContextualPanelManager : MonoBehaviour
{

    public EditSubactionContextPanel defaultSubactionContextPanel;
    public SubactionToContextPanelDict uniqueContextPanels;
    
    public void OnModelChanged()
    {
        if (LegacyEditorData.instance.currentSubactionDirty)
        {
            SubactionData subaction = LegacyEditorData.instance.currentSubaction;
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
                } else
                {
                    if (LegacyEditorData.contextualPanel != null)
                    {
                        LegacyEditorData.contextualPanel.DeactivatePanel();
                    }
                    defaultSubactionContextPanel.ActivatePanel();
                }
            }
        }
    }
}


/// <summary>
/// This derivation is necessary for the Serializable Dictionary to drawn in inspector
/// </summary>
[System.Serializable]
public class SubactionToContextPanelDict : SerializableDictionary<string, EditSubactionContextPanel>{}