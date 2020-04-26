using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class MenuSpriteColorSetter : MonoBehaviour {
    public ObjectToColor color_object;

    public enum ObjectToColor { Sprite, Text, Both };

    public bool always_set;

    private Image buttonBg;
    private TextMeshProUGUI text;

    void Start()
    {
        buttonBg = GetComponentInChildren<Image>();
        text = GetComponentInChildren<TextMeshProUGUI>();
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
