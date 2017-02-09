using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TextChanger : MonoBehaviour
{

    private Button buttonComponent;
    private Text textComponent;
    private Image imageComponent;
    private starSpawner stars;

    private Color hsvColor;

    // Use this for initialization
    void Start()
    {
        buttonComponent = GetComponent<Button>();
        textComponent = GetComponent<Text>();
        imageComponent = GetComponent<Image>();
        stars = Camera.main.GetComponent<starSpawner>();
    }

    // Update is called once per frame
    void Update()
    {
        hsvColor = stars.getColor();
        if (buttonComponent != null)
        {
            ColorBlock cb = buttonComponent.colors;
            cb.highlightedColor = hsvColor;
            buttonComponent.colors = cb;
        }
        if (imageComponent != null)
        {
            imageComponent.color = hsvColor;
        }
        //if (textComponent != null)
        //{
        //    textComponent.color = hsvColor;
        //}
    }
}
