using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EditSubactionContextPanel : ContextualPanelData {

	public void OnModelChanged()
    {
        if (LegacyEditorData.instance.currentSubaction != null)
        {
            LegacyEditorData.Unbanish(gameObject);
            LegacyEditorData.contextualPanel = this;
        } else
        {
            LegacyEditorData.Banish(gameObject);
            //LegacyEditorData.contextualPanel = null;
        }
    }

    public override void FireContextualPanelChange()
    {
        BroadcastMessage("OnContextualPanelChanged");

        //After the broadcast, clear all the "dirty" bits
    }
}
