using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EditSubactionContextPanel : ContextualPanelData {
    
    public void ActivatePanel()
    {
        if (LegacyEditorData.contextualPanel != this)
        {
            LegacyEditorData.Unbanish(gameObject);
            LegacyEditorData.contextualPanel = this;
            //Since this activation gets called in the middle of an update, the panel we just activated isn't getting the function call.
            BroadcastMessage("OnModelChanged");
            FireContextualPanelChange();
        }
    }

    public void DeactivatePanel()
    {
        LegacyEditorData.Banish(gameObject);
        FireContextualPanelChange();
    }
    
    public void OnModelChanged()
    {
        if (LegacyEditorData.instance.currentSubaction != null)
        {
            //ActivatePanel();
        }
        else
        {
            DeactivatePanel();
        }
    }

    public override void FireContextualPanelChange()
    {
        BroadcastMessage("OnContextualPanelChanged",SendMessageOptions.DontRequireReceiver);

        //After the broadcast, clear all the "dirty" bits
    }
}
