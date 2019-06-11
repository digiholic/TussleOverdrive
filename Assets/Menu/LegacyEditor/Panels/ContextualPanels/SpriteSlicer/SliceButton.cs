using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// This class currently just updates the text on the slice button to say either "Slice" or "Slice All" depending on
/// if a specific image file is selected for editing
/// </summary>
public class SliceButton : LegacyEditorWidget
{
    public UILabel label;

    private void Start()
    {
        ImageChanged(editor.currentImageFile);
    }

    void ImageChanged(object info)
    {
        //Since all we need to know is if it's null or not, we really don't care that it's a FileInfo
        //Not sure if that's good practice or if there's a cost to importing a library, but whatevs.
        if (info == null)
        {
            label.text = "Slice All";
        } else
        {
            label.text = "Slice";
        }
    }

    public override void RegisterListeners()
    {
        editor.CurrentImageFileChangedEvent += ImageChanged;
    }

    public override void UnregisterListeners()
    {
        editor.CurrentImageFileChangedEvent -= ImageChanged;
    }
}
