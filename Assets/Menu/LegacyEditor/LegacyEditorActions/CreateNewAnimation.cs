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
        LegacyEditorData.instance.loadedSpriteInfo.AddAnimation(animationToAdd, false);
        LegacyEditorData.ChangedAnimation();
    }

    public override void undo()
    {
        LegacyEditorData.instance.loadedSpriteInfo.DeleteAnimation(animationToAdd);
        LegacyEditorData.ChangedActionFile();
    }
}