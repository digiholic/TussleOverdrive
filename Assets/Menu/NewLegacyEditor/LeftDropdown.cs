using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LeftDropdown : MonoBehaviour {
    private UIPopupList list;

	// Use this for initialization
	void Awake () {
        list = GetComponent<UIPopupList>();
	}

    void Start()
    {
        foreach(string opt in LegacyEditorConstants.LeftDropdownOptions)
        {
            list.items.Add(opt);
        }
        UpdateOptionWithoutEvent();
        list.eventReceiver = gameObject;
    }

    void OnModelChanged()
    {
        if (LegacyEditorData.instance.leftDropdownDirty)
        {
            UpdateOptionWithoutEvent();
        }
    }

    //This is hacky as fuck, isn't it? I'm unsetting the event receiver so I can change this data without firing another change, preventing a double-fire and blowing up the redoList
    public void UpdateOptionWithoutEvent()
    {
        list.eventReceiver = null;
        list.selection = LegacyEditorData.instance.leftDropdown;
        list.eventReceiver = gameObject;
    }

    // Update is called once per frame
    void Update () {
		
	}

    void OnChangeDropdown(string selected)
    {
        //Create a message object to have the model execute
        ChangeLeftDropdownAction act = ScriptableObject.CreateInstance<ChangeLeftDropdownAction>();
        act.init(selected);
        LegacyEditorData.instance.DoAction(act);
    }
}
