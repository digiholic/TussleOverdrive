using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class PopulatePopupWithSprites : PopulatePopup
{
    private List<string> sprite_names = new List<string>();
    
    void OnFighterChanged(FighterInfo info)
    {
        List<ImageDefinition> sprites = new List<ImageDefinition>(LegacyEditorData.instance.loadedFighter.getSpriteData().sprites);
        sprite_names = sprites.Select(spriteData => spriteData.sprite_name).ToList();
        sprite_names.Sort();
        PopulateList(sprite_names);
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
