using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

[System.Serializable]
public class ActionFile
{
    public List<DynamicAction> actions = new List<DynamicAction>();

    /// <summary>
    /// Adds an action to this ActionFile. Overwrites if it exists
    /// </summary>
    /// <param name="newAction">The DynamicAction object to add.</param>
    public void Add(DynamicAction newAction) //Overwrites an action with the given name if it exists. Otherwise, adds it
    {
        actions.RemoveAll(s => s.name == newAction.name); //Removes all objects that have the same name as the new action
        actions.Add(newAction);
    }

    public void Delete(DynamicAction action)
    {
        actions.Remove(action);
    }

    public void Delete(string action_name)
    {
        foreach (DynamicAction act in actions)
        {
            if (act.name == action_name)
            {
                Delete(act);
                return;
            }
        }
    }

    public DynamicAction Get(string name)
    {
        foreach (DynamicAction action in actions)
        {
            if (action.name == name)
                return action;
        }
        //Debug.LogWarning("Could not find action " + name + " in ActionFile");
        return new DynamicAction("Null");
    }

    public void WriteJSON(string path)
    {
        string thisjson = JsonUtility.ToJson(this, true);
        Debug.Log(path);
        File.WriteAllText(path, thisjson);
    }
}

[System.Serializable]
public class DynamicAction
{
    public string name;
    public int length;
    public string sprite;
    public int sprite_rate;
    public bool loop;
    public string exit_action;

    public SubActionGroup set_up_subactions = new SubActionGroup();
    public SubActionGroup state_transition_subactions = new SubActionGroup();
    public SubActionGroup subactions_on_frame = new SubActionGroup();
    public SubActionGroup tear_down_subactions = new SubActionGroup();

    public DynamicAction(string _name, int _length = 1, string _sprite = "idle", int _sprite_rate = 1, bool _loop = false, string _exit_action = "NeutralAction")
    {
        name = _name;
        length = _length;
        sprite = _sprite;
        sprite_rate = _sprite_rate;
        loop = _loop;
        exit_action = _exit_action;
    }
}

[System.Serializable]
public class ActionGroup
{
    public List<string> subactions = new List<string>();

}

[System.Serializable]
public class ActionFrameGroup : ActionGroup
{
    public string frames; //Used only for actions_at_frame, parses the string to see if the current frame is in the allowed list

    public List<int> GetFrameNumbers()
    {
        List<int> frameNo = new List<int>();
        if (frames.Contains(",")) //If it's a comma seperated list
        {
            string[] frameStrings = frames.Split(',');
            foreach (string frameString in frameStrings)
            {
                frameNo.Add(int.Parse(frameString));
            }
        }
        else if (frames.Contains("-")) //If it's a range
        {
            string[] endPoints = frames.Split('-');
            for (int i = int.Parse(endPoints[0]); i <= int.Parse(endPoints[1]); i++)
            {
                frameNo.Add(i);
            }
        }
        else
            frameNo.Add(int.Parse(frames));
        return frameNo;
    }
}


[System.Serializable]
public class SubActionGroup
{
    public List<Subaction> subactions = new List<Subaction>();
}