using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeleteAnimation : LegacyEditorAction
{
    public AnimationDefinition animationToDelete = null;
    private AnimationDefinition previousAnimation = null;
    private int previousIndex = 0;

    public void init(AnimationDefinition def)
    {
        animationToDelete = def;
    }

    public override void execute()
    {
        //First we have to change to a null action, then we can delete the old one.
        previousAnimation = animationToDelete;
        LegacyEditorData.instance.currentAnimation = AnimationDefinition.NullAnimation; //Set to a null action
        
        previousIndex = LegacyEditorData.instance.loadedSpriteInfo.animations.IndexOf(animationToDelete);
        LegacyEditorData.instance.loadedSpriteInfo.DeleteAnimation(animationToDelete);
    }

    public override void undo()
    {
        LegacyEditorData.instance.currentAnimation = previousAnimation;
        LegacyEditorData.instance.loadedSpriteInfo.animations.Insert(previousIndex, previousAnimation); //Add it back where it was
    }
}
