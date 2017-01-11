using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PercentIcon : MonoBehaviour {
    public AbstractFighter fighter;

    private Text textComponent;
	// Use this for initialization
	void Start () {
        textComponent = transform.FindChild("Percent").GetComponent<Text>();
	}
	
	// Update is called once per frame
	void Update () {
        int damage = Mathf.FloorToInt(fighter.damage_percent);
        textComponent.text = damage.ToString() + "%";
        float r = Mathf.Min(1.0f, damage / 300.0f);
        textComponent.color = new Color(1.0f, 1.0f - r, 1.0f - r);
	}
}
