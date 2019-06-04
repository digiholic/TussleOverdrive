using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChangeCurrentAnimationAction : LegacyEditorAction
{
    private AnimationDefinition previousAnimation = null;
    //TODO store this for undoing once I have decided on how it'll be selected
    //private ImageDefinition previousImage = null;
    public AnimationDefinition nextAnimation;

    public void init(AnimationDefinition anim)
    {
        nextAnimation = anim;
    }

    public override void execute()
    {
        AnimationDefinition animToSet = nextAnimation;
        //If we're re-selecting the current action, unselect it instead (by creating an empty action that can be edited and saved later)
        previousAnimation = LegacyEditorData.instance.currentAnimation;
        //previousImage = LegacyEditorData.instance.currentImage;
        if (previousAnimation == nextAnimation)
        {
            animToSet = null;
        }
        LegacyEditorData.instance.currentAnimation = animToSet;
    }

    public override void undo()
    {
        LegacyEditorData.instance.currentAnimation = previousAnimation;
    }
}
