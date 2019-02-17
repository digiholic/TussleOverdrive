using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EditSubactionContextPanel : ContextualPanelData {
    
    public void OnModelChanged()
    {
        //IF we're still active and there's no subaction selected
        if (LegacyEditorData.instance.currentSubaction == null)
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
