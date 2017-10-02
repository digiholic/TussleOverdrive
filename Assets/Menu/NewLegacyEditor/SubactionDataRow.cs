using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SubactionDataRow : MonoBehaviour {
    public static SubactionDataRow current_selection;

    public Subaction subaction;
    public UILabel name_text;

    public bool selected;

    void OnPressed()
    {
        if (!selected) Select();
    }

    public void SetText(string text)
    {
        name_text.text = text;
    }

    public void Select()
    {
        if (current_selection != null) current_selection.Deselect();
        selected = true;
        current_selection = this;
    }

    public void Deselect()
    {
        selected = false;
        current_selection = null;
    }
}