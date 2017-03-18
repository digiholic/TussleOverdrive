using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuColorChanger : MonoBehaviour {

    public float hue = 0.0f; //hue hue hue
    public Color hsvColor = Color.black;

    
    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        hue = (hue + 0.05f * Time.deltaTime) % 1.0f;
        hsvColor = Color.HSVToRGB(hue, 0.8f, 1.0f);
    }

    public Color getColor()
    {
        return hsvColor;
    }
}
