using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuColorChanger : MonoBehaviour {
    public static MenuColorChanger menu_color;

    public float hue = 0.0f; //hue hue hue
    public Color hsvColor = Color.black;


    void Awake()
    {
        DontDestroyOnLoad(gameObject);
        if (menu_color == null) //if we don't have a settings object
            menu_color = this;
        
        else //if it's already set
            Destroy(gameObject); //Destroy the new one
    }

    // Use this for initialization
    void Start () {
        DontDestroyOnLoad(gameObject);
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
