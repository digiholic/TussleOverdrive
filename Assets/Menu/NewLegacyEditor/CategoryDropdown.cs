using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CategoryDropdown : MonoBehaviour {
    public UIPopupList popup;
    public UILabel display_text;

    public List<PanelInfo> panels;

    public string selected_string;

    void Start()
    {
        popup = GetComponent<UIPopupList>();
        popup.onSelectionChange = OnChangeDropdown;
        display_text = GetComponentInChildren<UILabel>();
        foreach (PanelInfo panel in panels)
        {
            popup.items.Add(panel.name);
        }
        display_text.text = popup.items[0];
    }

    void OnChangeDropdown(string item)
    {
        display_text.text = item;
        selected_string = item;
        foreach(PanelInfo panel in panels)
        {
            NGUITools.SetActive(panel.panel, (panel.name == item));
        }
    }
}

[System.Serializable]
public struct PanelInfo
{
    public string name;
    public GameObject panel;
}

