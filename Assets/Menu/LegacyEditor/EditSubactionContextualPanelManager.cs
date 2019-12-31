using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EditSubactionContextualPanelManager : LegacyEditorWidget
{

    public EditSubactionContextPanel defaultSubactionContextPanel;
    public SubactionToContextPanelDict uniqueContextPanels;

    void OnSubactionChanged(SubactionData subaction)
    {
        //Debug.Log("Subaction Changed: " + subaction,this);
        if (subaction == null)
        {
            //Debug.Log("Deactivating default", defaultSubactionContextPanel);
            foreach (EditSubactionContextPanel uniquePanel in uniqueContextPanels.Values){
                uniquePanel.DeactivatePanel();
            }
            defaultSubactionContextPanel.DeactivatePanel();
        }
        else
        {
            if (uniqueContextPanels.ContainsKey(subaction.SubactionName))
            {
                if (LegacyEditorData.contextualPanel != null)
                {
                    //Debug.Log("Deactivating old panel for unique", LegacyEditorData.contextualPanel);
                    LegacyEditorData.contextualPanel.DeactivatePanel();
                }
                //Debug.Log("Activating unique panel", uniqueContextPanels[subaction.SubactionName]);
                uniqueContextPanels[subaction.SubactionName].ActivatePanel();
            }
            else
            {
                if (LegacyEditorData.contextualPanel != null)
                {
                    //Debug.Log("Deactivating old panel for default", LegacyEditorData.contextualPanel);
                    LegacyEditorData.contextualPanel.DeactivatePanel();
                }
                //Debug.Log("Activating default panel", defaultSubactionContextPanel);
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