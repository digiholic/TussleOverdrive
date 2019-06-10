using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class EditSpriteContextualPanelManager : LegacyEditorWidget
{
    public AutosliceAllContextPanel autosliceAllContextPanel;

    private void DeactivateAll()
    {
        autosliceAllContextPanel.DeactivatePanel();
    }

    private void ActivateSlicers()
    {
        DeactivateAll();
        if (editor.leftDropdown == "Sprites")
        {
            //If none of the images are selected, we show the autoslice all panel
            if (editor.currentImageFile == null)
            {
                autosliceAllContextPanel.ActivatePanel();
            }
            else
            {
                autosliceAllContextPanel.DeactivatePanel();
                //Activate individual slicer
            }
        }
    }

    void OnLeftDropdownChanged(string leftDropdown)
    {
        ActivateSlicers();
    }

    void OnImageFileChanged(FileInfo file)
    {
        ActivateSlicers();
    }

    public override void RegisterListeners()
    {
        editor.CurrentImageFileChangedEvent += OnImageFileChanged;
        editor.LeftDropdownChangedEvent += OnLeftDropdownChanged;
    }

    public override void UnregisterListeners()
    {
        editor.CurrentImageFileChangedEvent -= OnImageFileChanged;
        editor.LeftDropdownChangedEvent -= OnLeftDropdownChanged;
    }
}
