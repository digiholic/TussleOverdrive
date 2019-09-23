using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

/// <summary>
/// Dynamic Actions are the meat and potatoes of BattleObjects. They define a state for the object, and get updates every frame.
/// They contain various groups of Subactions that are all executed when their trigger or frame happens.
/// </summary>
[System.Serializable]
public class DynamicAction : IVarDataContainer
{
    public string name;
    public int length;
    public string animationName;
    public string exit_action;

    //Dictionary keys are the SubactionCategories defined in the SubactionGroup enum, or "Frame_XX" for each frame.
    public SubGroupDict subactionCategories = new SubGroupDict();
    public Dictionary<string, BattleObjectVarData> variables = new Dictionary<string, BattleObjectVarData>();

    public DynamicAction(string _name, int _length = 1, string _animation = "idle", string _exit_action = "NeutralAction")
    {
        name = _name;
        length = _length;
        animationName = _animation;
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
        animationName = sourceAction.animationName;
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
    #region VarData Functions
    //Initialize the variable if it's not set yet, then return it
    public BattleObjectVarData GetVar(string var_name)
    {
        if (!HasVar(var_name))
        {
            Debug.Log("Attempting to get variable without setting one: " + var_name);
            SetVar(var_name, null);
        }
        return variables[var_name];
    }


    public void SetVar(string var_name, object obj)
    {
        if (HasVar(var_name))
        {
            variables[var_name].SetData(obj);
        }
        else
        {
            variables[var_name] = new BattleObjectVarData(var_name, obj);
        }
    }

    public bool HasVar(string var_name)
    {
        if (variables.ContainsKey(var_name))
            return true;
        else
            return false;
    }

    /// <summary>
    /// Gets the variable with the given name from the variables list
    /// </summary>
    /// <param name="var_name">The name of the variable to pull</param>
    /// <returns>The variable from the dict as an object</returns>
    public object GetVarData(string var_name)
    {
        return GetVar(var_name).GetData();
    }
    #endregion

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

    public void Set(string key, List<SubactionData> subactions){
        this[key] = subactions;
    }
}

/// <summary>
/// In order to properly serialize a list to a Serializeable Dict, it needs to have a class
/// </summary>
[System.Serializable]
public class SubactionDataListStorage : SerializableDictionary.Storage<List<SubactionData>> { }
