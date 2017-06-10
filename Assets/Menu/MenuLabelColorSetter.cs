using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuLabelColorSetter : MonoBehaviour {
    private UILabel textBg;

    void Start()
    {
        textBg = GetComponentInChildren<UILabel>();
    }

    void SetColor(Color color)
    {
        textBg.color = color;
    }
}
