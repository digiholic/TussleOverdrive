using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CreateNewAnimation : LegacyEditorAction
{
    public AnimationDefinition animationToAdd = null;

    public void init(AnimationDefinition def)
    {
        animationToAdd = def;
    }
    public override void execute()
    {
        LegacyEditorData.instance.loadedFighter.sprite_info.AddAnimation(animationToAdd, false);
        LegacyEditorData.ChangedAnimation();
    }

    public override void undo()
    {
        LegacyEditorData.instance.loadedFighter.sprite_info.DeleteAnimation(animationToAdd);
        LegacyEditorData.ChangedActionFile();
    }
}