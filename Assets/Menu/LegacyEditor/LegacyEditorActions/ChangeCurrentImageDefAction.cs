using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChangeCurrentImageDefAction : LegacyEditorAction
{
    private ImageDefinition previousDef = null;
    //TODO store this for undoing once I have decided on how it'll be selected
    //private ImageDefinition previousImage = null;
    public ImageDefinition nextDef;

    public void init(ImageDefinition def)
    {
        nextDef = def;
    }

    public override void execute()
    {
        ImageDefinition defToSet = nextDef;
        //If we're re-selecting the current action, unselect it instead (by creating an empty action that can be edited and saved later)
        previousDef = LegacyEditorData.instance.currentImageDef;
        //previousImage = LegacyEditorData.instance.currentImage;
        if (previousDef == nextDef)
        {
            defToSet = null;
        }
        LegacyEditorData.instance.currentImageDef = defToSet;
    }

    public override void undo()
    {
        LegacyEditorData.instance.currentImageDef = previousDef;
    }
}