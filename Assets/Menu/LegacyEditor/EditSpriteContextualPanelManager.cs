using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class EditSpriteContextualPanelManager : LegacyEditorWidget
{
    public AutosliceContextPanel autosliceContextPanel;

    private void DeactivateAll()
    {
        autosliceContextPanel.DeactivatePanel();
    }

    private void ActivateSlicers()
    {
        DeactivateAll();
        if (editor.leftDropdown == "Sprites")
        {
            autosliceContextPanel.ActivatePanel();
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
