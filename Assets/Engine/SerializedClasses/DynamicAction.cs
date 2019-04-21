using System.Collections;
using System.Collections.Generic;
using System.Linq;
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
    public SubGroupDict subactionCategories = new SubGroupDict();
    
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

    /// <summary>
    /// Add a Subaction to the action in the given category. Optionally, insert the subaction at a given position
    /// Defaults to -1, which will be interpreted as 'add it to the end'
    /// </summary>
    /// <param name="category">The subaction category to add it to</param>
    /// <param name="subData">The SubactionData to add to the list</param>
    /// <param name="order">Where to put it in the list (zero indexed). The SubactionData at that position and everything afterwards will get bumped up one. -1 means add to the end</param>
    public void AddSubaction(string category, SubactionData subData,int position=LAST_POSITION)
    {
        List<SubactionData> subList = subactionCategories.GetIfKeyExists(category);
        if (position == LAST_POSITION)
        {
            subList.Add(subData);
        } else
        {
            subList.Insert(position, subData);
            for (int i = position + 1; i < subList.Count; i++)
            {
                subList[i].order += 1;
            }
        }
    }

    public void SortSubactions()
    {
        foreach (var item in subactionCategories)
        {
            item.Value.Sort((item1, item2) => item1.order.CompareTo(item2.order));
        }
    }

    public const int LAST_POSITION = -1;
    public static DynamicAction NullAction = new DynamicAction("null");
}

/// <summary>
/// This derivation is necessary for the Serializable Dictionary to drawn in inspector
/// </summary>
[System.Serializable]
public class SubGroupDict : SerializableDictionary<string, List<SubactionData>, SubactionDataListStorage> {
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

/// <summary>
/// In order to properly serialize a list to a Serializeable Dict, it needs to have a class
/// </summary>
[System.Serializable]
public class SubactionDataListStorage : SerializableDictionary.Storage<List<SubactionData>> { }
