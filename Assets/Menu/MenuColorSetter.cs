using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

/// <summary>
/// This class is going to involve hard coding some assumptions about what color you're setting to save on code.
/// </summary>
public class MenuColorSetter : MonoBehaviour {
    private Image[] ui_sprites;
    private TextMeshProUGUI[] ui_labels;
    private Button[] ui_buttons;
    private ParticleSystem particles;

    public bool ChangePressed;

	// Use this for initialization
	void Start () {
        ui_sprites = GetComponents<Image>();
        ui_labels = GetComponents<TextMeshProUGUI>();
        ui_buttons = GetComponents<Button>();
        particles = GetComponent<ParticleSystem>();
	}
	
	// Update is called once per frame
	void Update () {
        foreach (Image ui_sprite in ui_sprites) //Sprites that have this component get their color set
        {
            ui_sprite.color = MenuColorChanger.getColor();
        }
        foreach(Button ui_button in ui_buttons) //Buttons that have this component have their hover color set
        {   
            //ui_button.hover = MenuColorChanger.getColor();
            //if (ChangePressed)
            //    ui_button.pressed = MenuColorChanger.getColor();
        }
        if (particles != null)
        {
            var main = particles.main;
            main.startColor = MenuColorChanger.getColor();
        }
	}
}
