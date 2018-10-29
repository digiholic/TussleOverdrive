using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class DynamicAction
{
    public string name;
    public int length;
    public string sprite;
    public int sprite_rate;
    public bool loop;
    public string exit_action;

    //Dictionary keys are the SubactionCategories defined in the SubactionGroup enum, or "Frame_XX" for each frame.
    public SubGroupDict subactionCategories;
    
    public DynamicAction(string _name, int _length = 1, string _sprite = "idle", int _sprite_rate = 1, bool _loop = false, string _exit_action = "NeutralAction")
    {
        name = _name;
        length = _length;
        sprite = _sprite;
        sprite_rate = _sprite_rate;
        loop = _loop;
        exit_action = _exit_action;
    }

    /// <summary>
    /// Create a Dynamic Action that is a copy of the existing one.
    /// When it gets added to the action file, it will append _new to it so there will be no name conflict.
    /// We don't need to worry about that here.
    /// </summary>
    /// <param name="sourceAction"></param>
    public DynamicAction(DynamicAction sourceAction)
    {
        name = sourceAction.name;
        length = sourceAction.length;
        sprite = sourceAction.sprite;
        sprite_rate = sourceAction.sprite_rate;
        loop = sourceAction.loop;
        exit_action = sourceAction.exit_action;

        //TODO clones currently have no subactions. I'll figure this out later.
        //set_upsub_actions = sourceAction.set_up_subactions; etc.
    }
}

/// <summary>
/// This derivation is necessary for the Serializable Dictionary to drawn in inspector
/// </summary>
[System.Serializable]
public class SubGroupDict : SerializableDictionary<string, List<SubactionData>> {

    /// <summary>
    /// If the key is in the dict, get the list it points to. Create an empty list otherwise.
    /// Just a quick convenience method, since there'll be a key for every frame
    /// Empty lists should be cleaned up before serializing to save space
    /// </summary>
    /// <param name="key">The dict key to search for</param>
    /// <returns>The list from the dict if the key exists, empty list otherwise</returns>
    public List<SubactionData> GetIfKeyExists(string key)
    {
        if (ContainsKey(key))
        {
            return this[key];
        } else
        {
            this[key] = new List<SubactionData>();
            return this[key];
        }

    }
}
