using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LeftPanelToggler : MonoBehaviour {
    public GameObject actionsPanel;
    public GameObject methodsPanel;
    public GameObject spritesPanel;
    public GameObject animationsPanel;

    void OnModelChanged()
    {
        if (LegacyEditorData.instance.leftDropdownDirty)
        {
            NGUITools.SetActive(actionsPanel, false);
            NGUITools.SetActive(methodsPanel, false);
            NGUITools.SetActive(spritesPanel, false);
            NGUITools.SetActive(animationsPanel, false);

            switch (LegacyEditorData.instance.leftDropdown)
            {
                case "Actions":
                    NGUITools.SetActive(actionsPanel, true);
                    break;
                case "Methods":
                    NGUITools.SetActive(methodsPanel, true);
                    break;
                case "Sprites":
                    NGUITools.SetActive(spritesPanel, true);
                    break;
                case "Animations":
                    NGUITools.SetActive(animationsPanel, true);
                    break;
                default:
                    break;
            }
        }
    }
}
