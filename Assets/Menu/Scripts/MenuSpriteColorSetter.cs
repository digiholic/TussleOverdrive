using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuSpriteColorSetter : MonoBehaviour {
    public ObjectToColor color_object;

    public enum ObjectToColor { Sprite, Text, Both };

    public bool always_set;

    private UISprite buttonBg;
    private UILabel text;

    void Start()
    {
        buttonBg = GetComponentInChildren<UISprite>();
        text = GetComponentInChildren<UILabel>();
    }

    void Update()
    {
        if (always_set) SetColor(MenuColorChanger.getColor());
    }

    void SetColor(Color color)
    {
        if (buttonBg != null && (color_object == ObjectToColor.Sprite || color_object == ObjectToColor.Both))
            buttonBg.color = color;
        if (text != null && (color_object == ObjectToColor.Text || color_object == ObjectToColor.Both))
            text.color = color;
    }
}
