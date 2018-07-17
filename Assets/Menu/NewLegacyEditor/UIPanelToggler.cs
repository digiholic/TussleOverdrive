using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class UIPanelToggler : MonoBehaviour {
    public bool panel_active = true;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        NGUITools.SetActiveChildren(gameObject, panel_active);
    }
}
