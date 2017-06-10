using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuSpriteColorSetter : MonoBehaviour {
    private UISprite buttonBg;

    void Start()
    {
        buttonBg = GetComponentInChildren<UISprite>();
    }

    void SetColor(Color color)
    {
        buttonBg.color = color;
    }
}
