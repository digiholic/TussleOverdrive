using System.Collections;
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
            Debug.Log("Banishing an already banished panel: " + panelToBanish);
            return;
        }
        //Set the panel and its old parent in the dictionary
        panelToParentDict.Add(panelToBanish, panelToBanish.transform.parent.gameObject);
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
            Debug.LogWarning("Attempting to Unbanish panel that wasn't banished: " + panelToUnbanish);
            return;
        }
        //Get the panel's old parent
        GameObject parent = panelToParentDict[panelToUnbanish];
        //Update it to that object and maintain it's relative position
        Vector3 oldPosition = panelToUnbanish.transform.localPosition;
        panelToUnbanish.transform.SetParent(parent.transform);
        panelToUnbanish.transform.localPosition = oldPosition;
        //Remove it from our list
        panelToParentDict.Remove(panelToUnbanish);
    }
}