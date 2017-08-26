using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SettingsPanelRig : MonoBehaviour {
    private static Dictionary<string, SettingsPanel> panels = new Dictionary<string, SettingsPanel>();

	// Use this for initialization
	void Start () {
	    foreach (SettingsPanel panel in GetComponentsInChildren<SettingsPanel>())
        {
            panels[panel.panelname] = panel;
        }
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public static SettingsPanel GetPanel(string name)
    {
        panels[name].starting_selection.Select();
        return panels[name];
    }
}
