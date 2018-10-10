using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SubactionTypeRig : MonoBehaviour {
    public GameObject typeButtonPrefab;

    private List<GameObject> children = new List<GameObject>();
    private UIGrid grid;

	// Use this for initialization
	void Awake () {
        grid = GetComponent<UIGrid>();

        foreach (SubactionType subType in Enum.GetValues(typeof(SubactionType)))
        {
            instantiateButton(subType);
        }
	}
	
    void OnModelChanged()
    {
        
    }

    private void instantiateButton(SubactionType subType)
    {
        GameObject go = NGUITools.AddChild(gameObject, typeButtonPrefab);
        SubactionTypeButton button = go.GetComponent<SubactionTypeButton>();
        button.SetSubType(subType);
        children.Add(go);
    }
}
