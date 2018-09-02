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

    public SubActionGroup set_up_subactions = new SubActionGroup();
    public SubActionGroup state_transition_subactions = new SubActionGroup();
    //public List<SubActionFrameGroup> subactions_on_frame = new List<SubActionFrameGroup>();
    public SubActionGroup subactions_on_frame = new SubActionGroup();
    public SubActionGroup tear_down_subactions = new SubActionGroup();

    private Dictionary<int, SubActionFrameGroup> subactions_at_frame;

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
    /// Create a Dynamic Action that is a copy of the existing one. The copy will change it's name
    /// to add _new so there is no name conflict
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

    public SubActionGroup GetGroup(string name)
    {
        switch (name)
        {
            case ("Set Up"):
                return set_up_subactions;
            case ("Transitions"):
                return state_transition_subactions;
            case ("Tear Down"):
                return tear_down_subactions;
            case ("On Frame"):
                return subactions_on_frame;
            default:
                Debug.LogError("Incorrect Subaction Group! " + name);
                return new SubActionGroup();
        }
    }

    /*
    protected void BuildDict()
    {
        subactions_at_frame.Clear();
        foreach (SubActionFrameGroup data in subactions_on_frame)
        {
            subactions_at_frame[data.frame] = data;
        }
    }
    */

    public void ReconcileSubactions()
    {
        set_up_subactions.ReconcileSubactions();
        state_transition_subactions.ReconcileSubactions();
        tear_down_subactions.ReconcileSubactions();
        subactions_on_frame.ReconcileSubactions();
        /*
        foreach (SubActionFrameGroup group in subactions_on_frame)
        {
            group.ReconcileSubactions();
        }
        */
    }
}
