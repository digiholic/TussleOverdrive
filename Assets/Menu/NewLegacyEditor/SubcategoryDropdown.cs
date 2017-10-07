using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SubcategoryDropdown : MonoBehaviour {
    public UIPopupList popup;
    public UILabel display_text;

    public List<SubPanelInfo> panels;

    public string selected_string;

    void Start()
    {
        popup = GetComponent<UIPopupList>();
        popup.onSelectionChange = OnChangeDropdown;
        display_text = GetComponentInChildren<UILabel>();
        foreach (SubPanelInfo panel in panels)
        {
            popup.items.Add(panel.name);
        }
        display_text.text = popup.items[0];
    }

    void OnChangeDropdown(string item)
    {
        display_text.text = item;
        selected_string = item;
        foreach (SubPanelInfo panel in panels)
        {
            NGUITools.SetActive(panel.panel, (panel.name == item));
        }
    }
}

[System.Serializable]
public struct SubPanelInfo
{
    public string name;
    public GameObject panel;
}
