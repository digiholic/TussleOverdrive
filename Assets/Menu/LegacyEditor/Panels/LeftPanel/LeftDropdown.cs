using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LeftDropdown : LegacyEditorWidget {
    private UIPopupList list;
    private UILabel label;

	// Use this for initialization
	void Awake () {
        list = GetComponent<UIPopupList>();
        label = GetComponentInChildren<UILabel>();
    }

    void Start()
    {
        foreach(string opt in LegacyEditorConstants.LeftDropdownOptions)
        {
            list.items.Add(opt);
        }
    }

    void OnLeftDropdownChanged(string s)
    {
        label.text = s;
        list.Set(s, false);
    }
    
    public void OnChangeDropdown()
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
