using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// This class is going to involve hard coding some assumptions about what color you're setting to save on code.
/// </summary>
public class MenuColorSetter : MonoBehaviour {
    private MenuColorChanger menu_color;

    private UISprite[] ui_sprites;
    private UILabel[] ui_labels;
    private UIButton[] ui_buttons;
    private ParticleSystem particles;

    public bool ChangePressed;

	// Use this for initialization
	void Start () {
        menu_color = GameObject.FindObjectOfType<MenuColorChanger>();

        ui_sprites = GetComponents<UISprite>();
        ui_labels = GetComponents<UILabel>();
        ui_buttons = GetComponents<UIButton>();
        particles = GetComponent<ParticleSystem>();
	}
	
	// Update is called once per frame
	void Update () {
        foreach (UISprite ui_sprite in ui_sprites) //Sprites that have this component get their color set
        {
            ui_sprite.color = menu_color.getColor();
        }
        foreach(UIButton ui_button in ui_buttons) //Buttons that have this component have their hover color set
        {   
            ui_button.hover = menu_color.getColor();
            if (ChangePressed)
                ui_button.pressed = menu_color.getColor();
        }
        if (particles != null)
        {
            particles.startColor = menu_color.getColor();
        }
	}
}
