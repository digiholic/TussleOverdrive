using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FighterPanel : MonoBehaviour {
    public Text nameplate;
    public Text bio;

    private XMLLoader loader;

	// Use this for initialization
	void Awake () {
        loader = GetComponent<XMLLoader>();
        Debug.Log(loader);
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void ChangeFighter(string xml_data)
    {
        loader.LoadXML(xml_data);
        nameplate.text = loader.SelectSingleNode("//fighter/name").GetString();
        //bio.text = loader.SelectSingleNode("//fighter/bio").GetString();
    }
}
