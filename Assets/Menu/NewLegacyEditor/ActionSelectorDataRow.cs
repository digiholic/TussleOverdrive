using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActionSelectorDataRow : MonoBehaviour {
    public static ActionSelectorDataRow current_selection;

    public DynamicAction action;
    public UISprite selected_sprite;
    public UILabel name_text;

    public bool selected;

	// Use this for initialization
	void Start () {
        NGUITools.SetActive(selected_sprite.gameObject, selected);
        if (selected) current_selection = this;
    }
	
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
        LegacyEditor.editor.BroadcastMessage("ChangeSelectedAction", action,SendMessageOptions.DontRequireReceiver);
    }

    public void Deselect()
    {
        selected = false;
        NGUITools.SetActive(selected_sprite.gameObject, false);
        current_selection = null;
    }
}
