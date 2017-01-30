using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Action : ScriptableObject {

    public string sprite_name;
    public int sprite_rate = 1;
    public bool loop = false;
    public int length = 1;

    public int current_frame;

    public List<string> set_up_actions = new List<string>();

    public List<string> actions_before_frame = new List<string>();
    public Dictionary<int,List<string>> actions_at_frame = new Dictionary<int,List<string>>();
    public List<string> actions_after_frame = new List<string>();
    public List<string> actions_at_last_frame = new List<string>();
    public List<string> actions_on_clank = new List<string>();
    public List<string> actions_on_prevail = new List<string>();
    public List<string> state_transition_actions = new List<string>();
    public List<string> tear_down_actions = new List<string>();

    protected int last_frame;
    protected AbstractFighter actor;
    protected GameController game_controller;

    public bool cond = true;

    public virtual void SetUp (AbstractFighter _actor) {
        last_frame = length;
        actor = _actor;
        actor.ChangeSprite(sprite_name);
        game_controller = actor.game_controller;
        foreach (string subaction in set_up_actions)
        {
            if (cond)
                SubactionLoader.executeSubaction(subaction, actor, this);
        }
        
        
    }

    // Update is called once per frame
    public virtual void Update() {
        if (sprite_rate != 0) // if it's zero, no need to animate
        {
            int sprite_number = Mathf.FloorToInt(current_frame / sprite_rate);
            if (sprite_rate < 0)
                sprite_number = Mathf.FloorToInt(current_frame / sprite_rate) - 1;
            actor.ChangeSubimage(sprite_number, loop);
        }

        foreach (string subaction in actions_before_frame)
            if (cond)
                SubactionLoader.executeSubaction(subaction, actor, this);

        if (actions_at_frame.ContainsKey(current_frame))
            foreach (string subaction in actions_at_frame[current_frame])
                if (cond)
                    SubactionLoader.executeSubaction(subaction, actor, this);

        if (current_frame >= last_frame)
            this.OnLastFrame();
	}

    public virtual void OnLastFrame()
    {
        foreach (string subaction in actions_at_last_frame)
            if (cond)
                SubactionLoader.executeSubaction(subaction, actor, this);
    }

    public virtual void LateUpdate() //This way the frame gets incremented after everything else
    {
        foreach (string subaction in actions_after_frame)
            if (cond)
                SubactionLoader.executeSubaction(subaction, actor, this);
        current_frame++;
    }

    public virtual void TearDown(Action new_action)
    {
        foreach (string subaction in tear_down_actions)
            if (cond)
                SubactionLoader.executeSubaction(subaction, actor, this);
    }

    public virtual void stateTransitions()
    {
        foreach (string subaction in state_transition_actions)
            if (cond)
                SubactionLoader.executeSubaction(subaction, actor, this);
    }

    public virtual void onClank()
    {
        foreach (string subaction in actions_on_clank)
            if (cond)
                SubactionLoader.executeSubaction(subaction, actor, this);
    }

    public virtual void onPrevail()
    {
        foreach (string subaction in actions_on_prevail)
            if (cond)
                SubactionLoader.executeSubaction(subaction, actor, this);
    }
}
