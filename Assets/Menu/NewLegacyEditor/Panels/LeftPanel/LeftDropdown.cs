using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LeftDropdown : LegacyEditorWidget {
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
        //EventDelegate.Add(list.onChange, OnChangeDropdown); ^^
    }

    void OnLeftDropdownChanged(string s)
    {
        UpdateOptionWithoutEvent();
    }
    
    //This is hacky as fuck, isn't it? I'm unsetting the event receiver so I can change this data without firing another change, preventing a double-fire and blowing up the redoList
    public void UpdateOptionWithoutEvent()
    {
        /* ^^
        EventDelegate.Remove(list.onChange, OnChangeDropdown);
        //list.eventReceiver = null; ^^
        list.value = LegacyEditorData.instance.leftDropdown;
        //list.eventReceiver = gameObject; ^^
        EventDelegate.Set(list.onChange, OnChangeDropdown);
        */
    }
    
    void OnChangeDropdown()
    {
        string selected = UIPopupList.current.value;

        //Create a message object to have the model execute
        ChangeLeftDropdownAction act = ScriptableObject.CreateInstance<ChangeLeftDropdownAction>();
        act.init(selected);
        LegacyEditorData.instance.DoAction(act);
    }

    public override void RegisterListeners()
    {
        editor.LeftDropdownChangedEvent += OnLeftDropdownChanged;
    }

    public override void UnregisterListeners()
    {
        editor.LeftDropdownChangedEvent -= OnLeftDropdownChanged;
    }
}
