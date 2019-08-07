using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChangeSubimageOrderAction : LegacyEditorAction
{
    private AnimationDefinition animationToModify;
    private List<string> nextList;
    private List<string> previousList;

    public void init(List<string> nextList)
    {
        this.nextList = nextList;
    }

    public override void execute()
    {
        animationToModify = LegacyEditorData.instance.currentAnimation;
        previousList = animationToModify.subimages;
        animationToModify.subimages = nextList;
        LegacyEditorData.ChangedAnimation();
    }

    public override void undo()
    {
        animationToModify.subimages = previousList;
        LegacyEditorData.ChangedAnimation();
    }
}
