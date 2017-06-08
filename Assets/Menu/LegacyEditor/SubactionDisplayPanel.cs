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
                foreach (string subaction in action.set_up_actions.subactions)
                    InitButton(subaction);
                break;
            case "State Transitions":
                foreach (string subaction in action.state_transition_actions.subactions)
                    InitButton(subaction);
                break;
            case "Tear Down":
                foreach (string subaction in action.tear_down_actions.subactions)
                    InitButton(subaction);
                break;
            case "Before Each Frame":
                foreach (string subaction in action.actions_before_frame.subactions)
                    InitButton(subaction);
                break;
            case "After Each Frame":
                foreach (string subaction in action.actions_after_frame.subactions)
                    InitButton(subaction);
                break;
            case "On Last Frame":
                foreach (string subaction in action.actions_at_last_frame.subactions)
                    InitButton(subaction);
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
