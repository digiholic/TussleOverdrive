using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuOptionSelector : MonoBehaviour {
    public UIFont selectedFont;
    public Vector3 selectedSize;
    public UIFont deselectedFont;
    public Vector3 deselectedSize;
    public UISprite highlightSprite;
    public Vector3 highlightLocation;

    private MenuButtonNavigator nav;
    private UILabel text;

	// Use this for initialization
	void Start () {
        nav = GetComponent<MenuButtonNavigator>();
        text = GetComponent<UILabel>();
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
