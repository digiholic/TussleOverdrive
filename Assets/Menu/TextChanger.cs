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


    private float h = 0.0f;
    private float s = 0.8f;
    private float v = 1.0f;


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
        h = stars.getHue();
        if (buttonComponent != null)
        {
            ColorBlock cb = buttonComponent.colors;
            cb.highlightedColor = Color.HSVToRGB(h, s, v);
            buttonComponent.colors = cb;
        }
        if (imageComponent != null)
        {
            imageComponent.color = Color.HSVToRGB(h, s, v);
        }
        //if (textComponent != null)
        //{
        //    textComponent.color = Color.HSVToRGB(h, s, v);
        //}
    }
}
