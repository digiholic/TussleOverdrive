using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SubcategoryDropdown : MonoBehaviour {
    public UIPopupList popup;
    public UILabel display_text;
    public List<string> panels;
    public WindowType window_type;

    private string lastSelectedPanel;
    public string selected_string;
    
    void Start()
    {
        popup = GetComponent<UIPopupList>();
        popup.onSelectionChange = OnChangeDropdown;
        display_text = GetComponentInChildren<UILabel>();
        foreach (string panel in panels)
        {
            popup.items.Add(panel);
        }
        display_text.text = popup.items[0];
        OnChangeDropdown(popup.items[0]);
    }

    void OnChangeDropdown(string item)
    {
        display_text.text = item;
        selected_string = item;
        if (window_type == WindowType.MAIN)
        {
            LegacyEditor.ChangeWindow(item);
        }
        else if (window_type == WindowType.SUB)
        {
            LegacyEditor.ChangeSubWindow(item);
        }
    }
}
