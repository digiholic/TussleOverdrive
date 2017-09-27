using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SubactionDataRow : MonoBehaviour {
    public static SubactionDataRow current_selection;

    public Subaction subaction;
    public UISprite selected_sprite;
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
        NGUITools.SetActive(selected_sprite.gameObject, true);
        current_selection = this;
    }

    public void Deselect()
    {
        selected = false;
        NGUITools.SetActive(selected_sprite.gameObject, false);
        current_selection = null;
    }
}