using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CategoryDropdown : MonoBehaviour {
    public UIPopupList popup;
    public UILabel display_text;

    public List<string> items;
    public string selected_string;

    void Start()
    {
        popup = GetComponent<UIPopupList>();
        popup.onSelectionChange = OnChangeDropdown;
        display_text = GetComponentInChildren<UILabel>();
        popup.items = items;
        display_text.text = items[0];
    }

    void OnChangeDropdown(string item)
    {
        display_text.text = item;
        selected_string = item;
    }
}
