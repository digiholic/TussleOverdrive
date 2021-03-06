﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PanelHider : MonoBehaviour {
    public Dictionary<GameObject, GameObject> panelToParentDict = new Dictionary<GameObject, GameObject>();

	// Use this for initialization
	void Awake () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void Banish(GameObject panelToBanish)
    {
        //If it's already here, don't bother re-banishing it
        if (panelToBanish.transform.parent.Equals(gameObject.transform))
        { 
            return;
        }
        //Update it to this object and maintain it's relative position
        Vector3 oldPosition = panelToBanish.transform.localPosition;
        panelToBanish.transform.SetParent(gameObject.transform);
        panelToBanish.transform.localPosition = oldPosition;
    }

    public void Unbanish(GameObject panelToUnbanish)
    {
        //If it's not in the shadow realm, ignore this function
        if (!panelToParentDict.ContainsKey(panelToUnbanish))
        {
            return;
        }
        //Get the panel's old parent
        GameObject parent = panelToParentDict[panelToUnbanish];
        Unbanish(panelToUnbanish, parent);
    }

    public void Unbanish(GameObject panelToUnbanish, GameObject parent)
    {
        //Update it to that object and maintain it's relative position
        Vector3 oldPosition = panelToUnbanish.transform.localPosition;
        panelToUnbanish.transform.SetParent(parent.transform);
        panelToUnbanish.transform.localPosition = oldPosition;
    }
}