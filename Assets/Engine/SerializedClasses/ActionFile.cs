using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System;

/// <summary>
/// The ActionFile is the representation of the data that is used to build actions.
/// It is manipulated in the builder and Unity Editor, but once it's loaded, it should create a bunch of actions and then chill quietly until it needs to save data back to disk.
/// It has a number of helper methods that let you easily manipulate data in the object.
/// </summary>
[System.Serializable]
public class ActionFile
{
    public List<DynamicAction> actions = new List<DynamicAction>();

    //If you need to load an action file in the editor, use the path to get it. This won't get serialized.
    [SerializeField]
    private string fighterDirectory;
    [SerializeField]
    private string actionFileName;

    public void LoadFromTextAsset()
    {
        string dir = FileLoader.GetFighterPath(fighterDirectory);
        string combinedPath = Path.Combine(dir, actionFileName);
        if (File.Exists(combinedPath))
        {
            string json = File.ReadAllText(combinedPath);
            JsonUtility.FromJsonOverwrite(json,this);
            //In case the file we loaded didn't have everything in the right order already, sort them.
            SortAllActions();
        }
        else
        {
            Debug.LogWarning("No action file found at " + fighterDirectory + "/" + actionFileName);
        }
    }


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
            throw new System.Exception("Multiple Actions with the same name! I told you this would happen!");
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
                //TODO this name convention will chain forever if you keep cloning the same action. This might be a problem, but probably not.
                actions.Add(cloneAction);
            }
        }
        //If there aren't any existing actions, it's fine to just add this one in.
        else
        {
            actions.Add(newAction);
        }
    }

    public void Delete(DynamicAction action)
    {
        actions.Remove(action);
    }

    public void DeleteByName(string action_name)
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
        return DynamicAction.NullAction;
    }

    public DynamicAction GetFirst()
    {
        if (actions.Count > 0)
        {
            return actions[0];
        }
        else return DynamicAction.NullAction;
    }

    public void Save(string path)
    {
        AssignOrdersToSubactions();    
        WriteJSON(path);
    }

    private void AssignOrdersToSubactions()
    {
        foreach (DynamicAction act in actions)
        {
            foreach (KeyValuePair<string,List<SubactionData>> item in act.subactionCategories)
            {
                //Each group has its own ordering
                int orderCount = 0;
                foreach (SubactionData subData in item.Value)
                {
                    Debug.Log(orderCount);
                    subData.order = orderCount++;
                }
            }
        }
    }

    private void SortAllActions()
    {
        foreach(DynamicAction act in actions)
        {
            act.SortSubactions();
        }
    }

    public void WriteJSON(string path)
    {
        Debug.Log("Writing JSON for Action File to: "+path);
        string thisjson = JsonUtility.ToJson(this, true);
        File.WriteAllText(path, thisjson);
    }

    public static ActionFile LoadActionsFromFile(string directory, string filename = "fighter_actions.json")
    {
        string dir = FileLoader.GetFighterPath(directory);
        string combinedPath = Path.Combine(dir, filename);
        if (File.Exists(combinedPath))
        {
            string json = File.ReadAllText(combinedPath);
            ActionFile info = JsonUtility.FromJson<ActionFile>(json);
            info.SortAllActions();
            return info;
        }
        else
        {
            Debug.LogWarning("No action file found at " + directory + "/" + filename);
            return null;
        }
    }
}