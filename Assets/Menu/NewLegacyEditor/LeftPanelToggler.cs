using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LeftPanelToggler : MonoBehaviour {
    public GameObject actionsPanel;
    public GameObject methodsPanel;
    public GameObject spritesPanel;
    public GameObject animationsPanel;

    private void Start()
    {
        BanishPanels();
    }

    void OnModelChanged()
    {
        if (LegacyEditorData.instance.leftDropdownDirty)
        {
            BanishPanels();
        }
    }

    private void BanishPanels()
    {
        LegacyEditorData.Banish(actionsPanel);
        LegacyEditorData.Banish(methodsPanel);
        LegacyEditorData.Banish(spritesPanel);
        LegacyEditorData.Banish(animationsPanel);
        switch (LegacyEditorData.instance.leftDropdown)
        {
            case "Actions":
                LegacyEditorData.Unbanish(actionsPanel);
                break;
            case "Methods":
                LegacyEditorData.Unbanish(methodsPanel);
                break;
            case "Sprites":
                LegacyEditorData.Unbanish(spritesPanel);
                break;
            case "Animations":
                LegacyEditorData.Unbanish(animationsPanel);
                break;
            default:
                break;
        }
    }
}
