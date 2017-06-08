using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SubactionGroupSelector : MonoBehaviour {
    public static string group;

    private UIPopupList popup;

    void Awake()
    {
        popup = GetComponent<UIPopupList>();
        group = popup.selection;
    }

    void ActionChanged(string actionName)
    {
        //Change the group back to set up
        popup.selection = "Set Up";
        OnChangeGroup("Set Up");
    }

	void OnChangeGroup(string groupName)
    {
        group = groupName;
        transform.root.BroadcastMessage("GroupChanged",groupName);
    }
}
