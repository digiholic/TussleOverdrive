using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SubactionDisplayPanel : MonoBehaviour {
    private string action_name;
    private string group_name;
    private int index = 0;

    public SubactionButton button_prefab;

    private List<SubactionButton> child_buttons = new List<SubactionButton>();
    void ActionChanged(string name)
    {
        action_name = name;
    }

    void GroupChanged(string group)
    {
        //Remove the old children
        foreach (SubactionButton child in child_buttons)
            Destroy(child.gameObject);
        child_buttons.Clear();

        ActionFile actions = ActionFileEditor.action_file;
        DynamicAction action = actions.Get(action_name);
        index = 0;
        switch (group)
        {
            case "Set Up":
                foreach (Subaction subaction in action.set_up_subactions.subactions)
                    InitButton(subaction.SubactionName);
                break;
            case "State Transitions":
                foreach (Subaction subaction in action.state_transition_subactions.subactions)
                    InitButton(subaction.SubactionName);
                break;
            case "Tear Down":
                foreach (Subaction subaction in action.tear_down_subactions.subactions)
                    InitButton(subaction.SubactionName);
                break;
            case "On Frame":
                foreach (Subaction subaction in action.subactions_on_frame.subactions)
                    InitButton(subaction.SubactionName);
                break;
            default:
                break;
        }
    }

    private void InitButton(string subaction)
    {
        SubactionButton button = Instantiate<SubactionButton>(button_prefab);
        button.transform.SetParent(transform, false);
        button.LoadSubaction(subaction, index);
        child_buttons.Add(button);
        index++;
    }
}
