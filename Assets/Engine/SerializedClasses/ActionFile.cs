using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

[System.Serializable]
public class ActionFile
{
    public List<DynamicAction> actions = new List<DynamicAction>();

    [SerializeField]
    private TextAsset JSONFile;

    /// <summary>
    /// Adds an action to this ActionFile.
    /// </summary>
    /// <param name="newAction">The DynamicAction object to add.</param>
    /// <param name="overwrite">Whether or not to remove the action that exists with that name.</param>
    public void Add(DynamicAction newAction, bool overwrite = false) //Overwrites an action with the given name if it exists. Otherwise, adds it
    {
        //This should always only ever return one thing, otherwise something's gone wrong
        //Why didn't I use a set or dict? ¯\_(ツ)_/¯
        //TODO: make this a set
        List<DynamicAction> existingActions = actions.FindAll(s => s.name == newAction.name);
        if (existingActions.Count > 1)
        {
            throw new System.Exception("Multiple Actions with the same name! I told you this would happen! Fix it!");
        }

        //If we have existing actions, we need to figure out what to do with the old one based on the overwrite flag
        if (existingActions.Count > 0)
        {
            if (overwrite)
            {
                actions.RemoveAll(s => s.name == newAction.name); //Removes all objects that have the same name as the new action
                actions.Add(newAction);
            } else
            { 
                DynamicAction cloneAction = new DynamicAction(newAction);
                cloneAction.name += "_new";
                //TODO this will chain forever if you keep cloning the same action. This might be a problem, but probably not.
                actions.Add(cloneAction);
            }
        } else
        {
            actions.Add(newAction);
        }
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

    public void LoadFromTextAsset()
    {
        if (JSONFile != null)
        {
            JsonUtility.FromJsonOverwrite(JSONFile.text, this);
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
        File.WriteAllText(path, thisjson);
        Debug.Log(path);
        Debug.Log(thisjson);
    }

    /// <summary>
    /// Converts all subactions from the base subaction class to their actual selves. Only needs to be run once.
    /// </summary>
    public void ReconcileSubactions()
    {
        foreach (DynamicAction act in actions)
        {
            act.ReconcileSubactions();
        }
    }

    public static ActionFile LoadActionsFromFile(string directory, string filename = "fighter_actions.json")
    {
        string dir = FileLoader.GetFighterPath(directory);
        string combinedPath = Path.Combine(dir, filename);
        if (File.Exists(combinedPath))
        {
            string json = File.ReadAllText(combinedPath);
            ActionFile info = JsonUtility.FromJson<ActionFile>(json);
            return info;
        }
        else
        {
            Debug.LogWarning("No action file found at " + directory + "/" + filename);
            return null;
        }
    }
}

[System.Serializable]
public class SubActionGroup
{
    public List<Subaction> subactions = new List<Subaction>();

    public void ReconcileSubactions()
    {
        List<Subaction> newSubactions = new List<Subaction>();
        foreach (Subaction subact in subactions)
        {
            newSubactions.Add(SubactionFactory.LoadSubactionAs(subact, subact.SubactionName));
        }
    }
}

[System.Serializable]
public class SubActionFrameGroup : SubActionGroup
{
    public int frame;
}