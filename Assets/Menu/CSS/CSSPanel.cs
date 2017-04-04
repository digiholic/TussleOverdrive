using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CSSPanel : MonoBehaviour {
    private XMLLoader data_xml;
    private UITexture portraitTexture;

	// Use this for initialization
	void Start () {
        data_xml = GetComponent<XMLLoader>();
        portraitTexture = GetComponentInChildren<UITexture>();
        portraitTexture.mainTexture = data_xml.LoadResource<Texture>(data_xml.SelectSingleNode("//fighter/css_icon").GetString());

    }
	
	// Update is called once per frame
	void Update () {
		
	}
}
