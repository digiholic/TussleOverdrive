using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SubactionCreateButtonGrid : MonoBehaviour {
    private Dictionary<SubactionType, List<SubactionData>> subactionsByCategory = new Dictionary<SubactionType, List<SubactionData>>();

	// Use this for initialization
	void Start () {
        SubactionData[] data = Resources.LoadAll<SubactionData>("SubactionData");
        foreach (SubactionData sub in data)
        {
            if (!subactionsByCategory.ContainsKey(sub.subType))
                subactionsByCategory[sub.subType] = new List<SubactionData>();

            subactionsByCategory[sub.subType].Add(sub);
        }
        Debug.Log(subactionsByCategory);
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
