using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SubactionDataRowExpanded : MonoBehaviour {
    public GameObject argDataPrefab;

    public Subaction subaction; //The subaction this panel represents
    public UILabel subactionName; //The name label
    public DynamicGridLayout argumentRig; //Where the arguments are parented to

    public List<GameObject> argDataInserted = new List<GameObject>(); //The list of inserted data so we can clear it out when needed.

    public bool deleteMe;
	// Use this for initialization
	void Start () {
        subactionName.text = subaction.SubactionName;
        RefreshArguments();

	}
	
	// Update is called once per frame
	void Update () {
        if (deleteMe) DeleteSelf();
	}

    void RefreshArguments()
    {
        foreach (GameObject obj in argDataInserted)
        {
            NGUITools.Destroy(obj);
        }
        argDataInserted.Clear();

        foreach (SubactionVarData data in subaction.arg_list)
        {
            GameObject obj = NGUITools.AddChild(argumentRig.gameObject, argDataPrefab);
            ArgumentDataRow row = obj.GetComponent<ArgumentDataRow>();
            row.UpdateArgument(data);
            argumentRig.Reposition();
            argDataInserted.Add(obj);
        }
    }

    void DeleteSelf()
    {
        NGUITools.Destroy(this.gameObject);
    }
}
