using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SettingsPanelRig : MonoBehaviour {
    private static Dictionary<string, SettingsPanel> panels = new Dictionary<string, SettingsPanel>();

    private static SettingsPanel selected_panel;

	// Use this for initialization
	void Awake() {
	    foreach (SettingsPanel panel in GetComponentsInChildren<SettingsPanel>())
        {
            panels[panel.panelname] = panel;
        }
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public static void SelectPanel(string name)
    {
        if (selected_panel != null)
        {
            if (selected_panel == panels[name]) return;
            selected_panel.Disable();
        }
        panels[name].Enable();
        selected_panel = panels[name];
    }

    public static SettingsPanel GetPanel(string name)
    {
        panels[name].starting_selection.Select();
        return panels[name];
    }
}
