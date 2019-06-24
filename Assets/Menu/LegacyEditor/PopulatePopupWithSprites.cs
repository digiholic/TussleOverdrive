using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class PopulatePopupWithSprites : PopulatePopup
{
    private List<string> sprite_names = new List<string>();
    
    void OnFighterChanged(FighterInfo info)
    {
        Debug.Log("Attempting to populate with sprites but sprites are broken. This needs to be fixed soon", this);
        //List<ImageDefinition> sprites = new List<ImageDefinition>(LegacyEditorData.instance.loadedSpriteInfo.animations);
        //sprite_names = sprites.Select(spriteData => spriteData.ImageName).ToList();
        //sprite_names.Sort();
        //PopulateList(sprite_names);
    }

    public override void RegisterListeners()
    {
        editor.FighterInfoChangedEvent += OnFighterChanged;
    }

    public override void UnregisterListeners()
    {
        editor.FighterInfoChangedEvent -= OnFighterChanged;
    }
}
