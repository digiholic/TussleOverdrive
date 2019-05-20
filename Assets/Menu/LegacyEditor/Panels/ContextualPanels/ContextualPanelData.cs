using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// The contextual panel is similar to the LegacyEditorData on a smaller scale.
/// It has its own sets of data that it can manipulate and pass functionality
/// </summary>
public abstract class ContextualPanelData : LegacyEditorWidget {

    /// <summary>
    /// Calls all of the OnContextualPanelChanged methods in child objects of the contextual panel
    /// </summary>
    public abstract void FireContextualPanelChange();
    public bool debug = false;

    public static bool isOfType(System.Type panelType)
    {
        ContextualPanelData data = LegacyEditorData.contextualPanel;
        if (data == null) return false;
        return (data.GetType() == panelType);
    }

    public void ActivatePanel()
    {
        if (debug) Debug.Log("Activating Panel", this);

        LegacyEditorData.Unbanish(gameObject);
        
        if (LegacyEditorData.contextualPanel != this)
        {
            LegacyEditorData.contextualPanel = this;
            //Since this activation gets called in the middle of an update, the panel we just activated isn't getting the function call.
            BroadcastMessage("OnModelChanged");
            FireContextualPanelChange();

        }
    }

    public void DeactivatePanel()
    {
        if (debug) Debug.Log("Deactivating Panel", this);
        LegacyEditorData.Banish(gameObject);
        FireContextualPanelChange();
    }
}
