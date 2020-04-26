using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class MenuOptionSelector : MonoBehaviour {
    public TMP_FontAsset selectedFont;
    public Vector3 selectedSize;
    public TMP_FontAsset deselectedFont;
    public Vector3 deselectedSize;
    public Image highlightSprite;
    public Vector3 highlightLocation;

    private MenuButtonNavigator nav;
    private TextMeshProUGUI text;

	// Use this for initialization
	void Start () {
        nav = GetComponent<MenuButtonNavigator>();
        text = GetComponent<TextMeshProUGUI>();
	}
	
	// Update is called once per frame
	void Update () {
		if (nav.selected)
        {
            transform.localScale = Vector3.Lerp(transform.localScale, selectedSize, Time.deltaTime*5);
            highlightSprite.transform.localPosition = Vector3.Lerp(highlightSprite.transform.localPosition, highlightLocation, Time.deltaTime * 10);
            text.font = selectedFont;
        } else
        {
            transform.localScale = Vector3.Lerp(transform.localScale, deselectedSize, Time.deltaTime*5);
            text.font = deselectedFont;
        }
	}
}
