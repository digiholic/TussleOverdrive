using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class PopulatePopupWithSprites : PopulatePopup
{
    private List<string> sprite_names = new List<string>();
    
    private void OnModelChanged()
    {
        if (LegacyEditorData.instance.loadedFighterDirty)
        {
            List<SpriteData> sprites = new List<SpriteData>(LegacyEditorData.instance.loadedFighter.getSpriteData().sprites);
            sprite_names = sprites.Select(spriteData => spriteData.sprite_name).ToList();
            sprite_names.Sort();
            PopulateList(sprite_names);
        }
    }
}
