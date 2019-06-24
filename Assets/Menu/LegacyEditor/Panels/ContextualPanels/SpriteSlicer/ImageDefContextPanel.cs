using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ImageDefContextPanel : ContextualPanelData
{
    public void OnModelChanged()
    {
        //If this is still active but we're not looking at the sprites editor, deactivate it
        if (editor.leftDropdown != "Sprites" || editor.currentImageDef == null)
        {
            DeactivatePanel();
        }
    }

    public void DeleteImageDef()
    {
        editor.loadedSpriteInfo.imageDefinitions.Remove(editor.currentImageDef);
        editor.currentImageDef = null;
        LegacyEditorData.ChangedFighterData();
    }

    public override void FireContextualPanelChange()
    {
        BroadcastMessage("OnContextualPanelChanged", SendMessageOptions.DontRequireReceiver);
    }

    public override void RegisterListeners() { }

    public override void UnregisterListeners() { }

}
