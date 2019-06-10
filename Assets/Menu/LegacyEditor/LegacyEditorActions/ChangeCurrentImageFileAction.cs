using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class ChangeCurrentImageFileAction : LegacyEditorAction
{
    private FileInfo previousFile = null;
    //TODO store this for undoing once I have decided on how it'll be selected
    //private ImageDefinition previousImage = null;
    public FileInfo nextFile;

    public void init(FileInfo file)
    {
        nextFile = file;
    }

    public override void execute()
    {
        FileInfo fileToSet = nextFile;
        //If we're re-selecting the current action, unselect it instead (by creating an empty action that can be edited and saved later)
        previousFile = LegacyEditorData.instance.currentImageFile;
        //previousImage = LegacyEditorData.instance.currentImage;
        if (previousFile == nextFile)
        {
            fileToSet = null;
        }
        LegacyEditorData.instance.currentImageFile = fileToSet;
    }

    public override void undo()
    {
        LegacyEditorData.instance.currentImageFile = previousFile;
    }
}
