using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeleteSubimageAction : LegacyEditorAction
{
    private AnimationDefinition animationToModify;
    private int indexToRemove;
    private string removedSubimage;

    public void init(int index)
    {
        indexToRemove = index;
    }

    public override void execute()
    {
        animationToModify = LegacyEditorData.instance.currentAnimation;
        removedSubimage = animationToModify.subimages[indexToRemove];
        animationToModify.subimages.RemoveAt(indexToRemove);
        LegacyEditorData.ChangedAnimation();
    }

    public override void undo()
    {
        animationToModify.subimages.Insert(indexToRemove, removedSubimage);
        LegacyEditorData.ChangedAnimation();
    }
}
