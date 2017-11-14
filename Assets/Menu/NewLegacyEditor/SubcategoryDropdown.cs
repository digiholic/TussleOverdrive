using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SubcategoryDropdown : MonoBehaviour {
    public UIPopupList popup;
    public UILabel display_text;

    public List<SubPanelInfo> panels;

    private SubPanelInfo lastSelectedPanel;
    public string selected_string;

    void Start()
    {
        popup = GetComponent<UIPopupList>();
        popup.onSelectionChange = OnChangeDropdown;
        display_text = GetComponentInChildren<UILabel>();
        foreach (SubPanelInfo panel in panels)
        {
            popup.items.Add(panel.name);
            NGUITools.SetActive(panel.panel, false); //Initially turn everything off
        }
        display_text.text = popup.items[0];
        OnChangeDropdown(popup.items[0]);
    }

    void OnChangeDropdown(string item)
    {
        display_text.text = item;
        selected_string = item;
        if (lastSelectedPanel.panel != null)
            NGUITools.SetActive(lastSelectedPanel.panel, false); //Disable old panel
        foreach (SubPanelInfo panel in panels)
        {
            if (panel.name == item)
            {
                NGUITools.SetActive(panel.panel, true);
                lastSelectedPanel = panel;
            }
        }
    }

    void OnGroupChanged(string group)
    {
        Debug.Log("Dropdown changed: " + group);
        if (group != "Properties")
        {
            LegacyEditor.editor.current_group_name = group;
            LegacyEditor.SubActionGroupChanged();
        }
    }
}

[System.Serializable]
public struct SubPanelInfo
{
    public string name;
    public GameObject panel;
}
