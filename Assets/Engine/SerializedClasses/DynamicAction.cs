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

    //Dictionary keys are the SubactionCategories defined in the SubactionCategory enum, or "Frame_XX" for each frame.
    public SerializableDictionary<string, List<SubactionData>> subactionCategories;

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
