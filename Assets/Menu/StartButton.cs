using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StartButton : MonoBehaviour {

    private UILabel textComponent;

    private float alpha = 144.0f;
    private float alpha_rad = 0.0f;
	// Use this for initialization
	void Start () {
        textComponent = GetComponent<UILabel>();	
	}
	
	// Update is called once per frame
	void Update () {
        alpha = 127 * Mathf.Sin(alpha_rad) + 128;
        alpha_rad += 0.05f;
        textComponent.color = new Color(0, 0, 0, alpha / 255.0f);
	}
}
