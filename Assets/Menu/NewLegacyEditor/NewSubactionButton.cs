using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NewSubactionButton : MonoBehaviour {
    public string SubactionName;

    void Start()
    {
        GetComponentInChildren<UILabel>().text = SubactionName;
    }
	void OnPress(bool pressed)
    {
        if (pressed)
        {
            if (LegacyEditor.editor.subaction_group != null)
            {
                SubactionFactory.AddNewSubaction(SubactionName, LegacyEditor.editor.subaction_group);
                LegacyEditor.BroadcastSelectedActionChanged();
            }
        }
    }
}
