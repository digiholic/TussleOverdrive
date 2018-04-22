using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FooterButtons : MonoBehaviour {
    public List<GlyphWithText> buttons;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    [System.Serializable]
    public enum ButtonGlyph
    {
        DPAD,
        ABUTTON,
        SBUTTON
    }

    [System.Serializable]
    public struct GlyphWithText
    {
        public ButtonGlyph glyph;
        public string text;
    }
}
