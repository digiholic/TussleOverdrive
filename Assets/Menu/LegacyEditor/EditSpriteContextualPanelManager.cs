using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class EditSpriteContextualPanelManager : LegacyEditorWidget
{
    public AutosliceContextPanel autosliceContextPanel;
    public ImageDefContextPanel imageDefPanel;

    private void DeactivateAll()
    {
        autosliceContextPanel.DeactivatePanel();
        imageDefPanel.DeactivatePanel();
    }

    private void ActivateSlicers()
    {
        DeactivateAll();
        if (editor.leftDropdown == "Sprites")
        {
            if (editor.currentImageDef == null || editor.currentImageDef?.ImageName == "")
            {
                autosliceContextPanel.ActivatePanel();
            }
            else
            {
                imageDefPanel.ActivatePanel();
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

    void OnImageDefChanged(ImageDefinition def)
    {
        ActivateSlicers();
    }

    public override void RegisterListeners()
    {
        editor.CurrentImageFileChangedEvent += OnImageFileChanged;
        editor.LeftDropdownChangedEvent += OnLeftDropdownChanged;
        editor.CurrentImageDefinitionChangedEvent += OnImageDefChanged;
    }

    public override void UnregisterListeners()
    {
        editor.CurrentImageFileChangedEvent -= OnImageFileChanged;
        editor.LeftDropdownChangedEvent -= OnLeftDropdownChanged;
        editor.CurrentImageDefinitionChangedEvent -= OnImageDefChanged;
    }
}
