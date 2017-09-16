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
	
    public void LoadFighter(FighterInfo info)
    {
        current_fighter = info;
        current_actions = info.action_file;
    }

    void SaveFighter()
    {

    }
}
