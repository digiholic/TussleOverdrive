using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PropertyLayout : MonoBehaviour, LegacyDataViewer {
    public UIInput propName;
    public UIInput length;
    public UIInput sprite;
    public UIInput sprite_rate;
    public UICheckbox loop;
    public UIInput exit_action;


    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void OnEnable() {
        //LegacyEditor.OnSelectedActionChanged += SelectedActionChanged;
    }

    public void OnDisable()
    {
        //LegacyEditor.OnSelectedActionChanged -= SelectedActionChanged;
    }

    public void SelectedActionChanged(DynamicAction action) {
        propName.text = action.name;
        length.text = action.length.ToString();
        sprite.text = action.sprite;
        sprite_rate.text = action.sprite_rate.ToString();
        loop.isChecked = action.loop;
        exit_action.text = action.exit_action;
    }

    public void ActionFileChanged(ActionFile actions) {}
    public void CategoryChanged(string category_name) {}
    public void FighterChanged(FighterInfo fighter_info) {}
    public void SubWindowChanged(string sub_window_name) {}
    public void WindowChanged(string window_name) {}
}
