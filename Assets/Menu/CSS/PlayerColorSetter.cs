using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerColorSetter : MonoBehaviour {
    public int playerNum;

    private UISprite ui_sprite;
    private SpriteRenderer sprite_renderer;

	// Use this for initialization
	void Start () {
        ui_sprite = GetComponent<UISprite>();
        sprite_renderer = GetComponent<SpriteRenderer>();
        if (ui_sprite != null)
            ui_sprite.color = Settings.current_settings.player_colors[playerNum];
        if (sprite_renderer != null)
            sprite_renderer.color = Settings.current_settings.player_colors[playerNum];
	}
}
