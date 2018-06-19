using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// This class sets the sprite of the "border notch" on the settings menu. It contains a reference to the object with the border, and a sprite to change to.
/// It checks this component for a MenuButtonNavigator and fires when it's selected.
/// </summary>

[RequireComponent(typeof(MenuButtonNavigator))]
public class MenuNotchSetter : MonoBehaviour {
    private MenuButtonNavigator menu;
    [SerializeField]
    private Sprite borderSprite;
    [SerializeField]
    private SpriteRenderer borderObject;

	// Use this for initialization
	void Start () {
        menu = GetComponent<MenuButtonNavigator>();
	}
	
	// Update is called once per frame
	void Update () {
		if (menu.selected && borderObject.sprite != borderSprite)
        {
            borderObject.sprite = borderSprite;
        }
	}
}
