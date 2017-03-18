using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// This class is going to involve hard coding some assumptions about what color you're setting to save on code.
/// </summary>
public class MenuColorSetter : MonoBehaviour {
    private MenuColorChanger menu_color;

    private UISprite ui_sprite;
    private UILabel ui_label;
    private UIButton ui_button;

	// Use this for initialization
	void Start () {
        menu_color = GameObject.FindObjectOfType<MenuColorChanger>();

        ui_sprite = GetComponent<UISprite>();
        ui_label = GetComponent<UILabel>();
        ui_button = GetComponent<UIButton>();	
	}
	
	// Update is called once per frame
	void Update () {
        if (ui_sprite != null) //Sprites that have this component get their color set
        {
            ui_sprite.color = menu_color.getColor();
        }
        if (ui_button != null) //Buttons that have this component have their hover color set
        {
            
            ui_button.hover = menu_color.getColor();
        }
	}
}
