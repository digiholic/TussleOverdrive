using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SettingsPanel : MonoBehaviour {
    public static SettingsPanel active_panel;

    public string panelname;

    public MenuButtonNavigator settings_header;
    public MenuButtonNavigator starting_selection;

    private GameObject internal_panel;

    void Awake()
    {
        internal_panel = transform.Find("Panel").gameObject;
        internal_panel.SetActive(false);
    }

	public void Enable()
    {
        active_panel = this;
        internal_panel.SetActive(true);
    }

    public void Disable()
    {
        internal_panel.SetActive(false);
    }
}
