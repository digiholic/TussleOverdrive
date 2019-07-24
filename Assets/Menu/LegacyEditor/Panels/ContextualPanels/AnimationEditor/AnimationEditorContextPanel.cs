using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationEditorContextPanel : ContextualPanelData
{
    public void OnModelChanged()
    {
        //If this is still active but we're not looking at the sprites editor, deactivate it
        if (editor.leftDropdown != "Animations" || editor.rightDropdown != "Subimages")
        {
            DeactivatePanel();
        }
    }

    public override void FireContextualPanelChange()
    {
        BroadcastMessage("OnContextualPanelChanged", SendMessageOptions.DontRequireReceiver);

        //After the broadcast, clear all the "dirty" bits
    }

    public override void RegisterListeners()
    {
        
    }

    public override void UnregisterListeners()
    {
        
    }
}
