using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Enables the button or UI element only if the fighter is loaded.
/// </summary>
public class EnableOnFighterLoad : MonoBehaviour {
    private Collider col;

	// Use this for initialization
	void Start () {
        col = GetComponent<Collider>();
	}
	
	// Update is called once per frame
	void Update () {
        if (LegacyEditor.FighterLoaded)
            col.enabled = true;
        else
            col.enabled = false;
	}
}
