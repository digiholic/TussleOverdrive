using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LegacyEditor : MonoBehaviour {
    public static LegacyEditor editor;

    public FighterInfo current_fighter;
    public ActionFile current_actions;

	// Use this for initialization
	void Awake () {
        editor = this;
	}
	
    void LoadFighter(string path)
    {

    }
}
