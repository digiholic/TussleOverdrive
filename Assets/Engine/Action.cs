using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Diagnostics;
using System;

public class GameAction {
    private StackTrace stackTrace = new StackTrace();

    public string sprite_name;
    public int sprite_rate = 1;
    public bool loop = false;
    public int length = 1;
    public string exit_action = "NeutralAction";

    public int current_frame;

    public ActionGroup set_up_actions = new ActionGroup();
    public ActionGroup actions_before_frame = new ActionGroup();
    public Dictionary<int,ActionGroup> actions_at_frame = new Dictionary<int,ActionGroup>();
    public ActionGroup actions_after_frame = new ActionGroup();
    public ActionGroup actions_at_last_frame = new ActionGroup();
    public ActionGroup actions_on_clank = new ActionGroup();
    public ActionGroup actions_on_prevail = new ActionGroup();
    public ActionGroup state_transition_actions = new ActionGroup();
    public ActionGroup tear_down_actions = new ActionGroup();

    public Dictionary<string, Hitbox> hitboxes = new Dictionary<string, Hitbox>();
    public Dictionary<string, HitboxLock> hitbox_locks = new Dictionary<string, HitboxLock>();

    protected int last_frame;
    protected BattleObject actor;
    protected BattleController game_controller;
    
    public List<bool> cond_list = new List<bool> { true };
    public int cond_depth = 0;

    public Dictionary<string, object> variable = new Dictionary<string, object>();
    public Dictionary<string, object> variables_to_pass = new Dictionary<string, object>();
     
    public virtual void SetUp (BattleObject obj) {
        last_frame = length;
        actor = obj;
        actor.BroadcastMessage("ChangeSprite",sprite_name);
        game_controller = BattleController.current_battle;
        foreach (string subaction in set_up_actions.subactions)
            CheckCondAndExecute(subaction);
    }

    // Update is called once per frame
    public virtual void Update() {
        if (sprite_rate != 0) // if it's zero, no need to animate
        {
            int sprite_number = Mathf.FloorToInt(current_frame / sprite_rate);
            if (sprite_rate < 0)
                sprite_number = Mathf.FloorToInt(current_frame / sprite_rate) - 1;
            actor.GetSpriteHandler().ChangeSubimage(sprite_number, loop);
        }

        foreach (string subaction in actions_before_frame.subactions)
            CheckCondAndExecute(subaction);

        if (actions_at_frame.ContainsKey(current_frame))
            foreach (string subaction in actions_at_frame[current_frame].subactions)
                CheckCondAndExecute(subaction);

        if (current_frame >= last_frame)
            this.OnLastFrame();
	}

    public virtual void OnLastFrame()
    {
        foreach (string subaction in actions_at_last_frame.subactions)
            CheckCondAndExecute(subaction);
        if (exit_action != null)
            actor.SendMessage("DoAction", exit_action);
    }

    public virtual void LateUpdate() //This way the frame gets incremented after everything else
    {
        foreach (string subaction in actions_after_frame.subactions)
            CheckCondAndExecute(subaction);
        current_frame++;
    }

    public virtual void TearDown(GameAction new_action)
    {
        //Pass the variables we've set to pass to the next action
        foreach (KeyValuePair<string,object> passvar in variables_to_pass)
        {
            new_action.SetVar(passvar.Key, passvar.Value);
        }

        //Deactivate and destroy hitboxes at the end of the action
        foreach (Hitbox hbox in hitboxes.Values)
        {
            hbox.Deactivate();
            GameObject.Destroy(hbox.gameObject);
        }
        foreach (string subaction in tear_down_actions.subactions)
            CheckCondAndExecute(subaction);
    }

    public virtual void stateTransitions()
    {
        foreach (string subaction in state_transition_actions.subactions)
            CheckCondAndExecute(subaction);
    }

    public virtual void onClank()
    {
        foreach (string subaction in actions_on_clank.subactions)
            CheckCondAndExecute(subaction);
    }

    public virtual void onPrevail()
    {
        foreach (string subaction in actions_on_prevail.subactions)
            CheckCondAndExecute(subaction);
    }

    public void SetDynamicAction(DynamicAction dynAction)
    {
        length = dynAction.length;
        sprite_name = dynAction.sprite;
        sprite_rate = dynAction.sprite_rate;
        loop = dynAction.loop;
        exit_action = dynAction.exit_action;

        set_up_actions = dynAction.set_up_actions;
        actions_before_frame = dynAction.actions_before_frame;
        actions_at_frame = dynAction.actions_at_frame_dict;
        actions_after_frame = dynAction.actions_after_frame;
        actions_at_last_frame = dynAction.actions_at_last_frame;
        state_transition_actions = dynAction.state_transition_actions;
        tear_down_actions = dynAction.tear_down_actions;

    }
    
    public void AdjustLength(int new_length)
    {
        last_frame = new_length;
    }

    public bool HasVar(string var_name)
    {
        if (variable.ContainsKey(var_name))
            return true;
        else
            return false;
    }

    public void SetVar(string var_name, object obj)
    {
        variable[var_name] = obj;
    }

    /// <summary>
    /// Gets the variable with the given name from the variables list
    /// </summary>
    /// <param name="var_name">The name of the variable to pull</param>
    /// <returns>The variable from the dict as an object</returns>
    public object GetVar(string var_name)
    {
        if (variable.ContainsKey(var_name))
        {
            return variable[var_name];
        }
        else
        {
            UnityEngine.Debug.LogWarning("Could not find variable " + var_name + " in Action " + this.ToString());
            UnityEngine.Debug.Log(stackTrace.GetFrame(1).GetMethod().Name);
            return null;
        }
    }

    public int GetIntVar(string var_name)
    {
        return (int)GetVar(var_name);
    }

    public float GetFloatVar(string var_name)
    {
        return (float)GetVar(var_name);
    }

    public bool GetBoolVar(string var_name)
    {
        return (bool)GetVar(var_name);
    }

    public string GetStringVar(string var_name)
    {
        return (string)GetVar(var_name);
    }

    public void PassVariable(string var_name, object var_value)
    {
        variables_to_pass[var_name] = var_value;
    }

    private void CheckCondAndExecute(string subact)
    {
        if (!cond_list.Contains(false)) //If there are no falses in the list, execute the action
        {
            SubactionLoader.executeSubaction(subact, actor, this);
        }
        else
        {
            //Check if the subaction is one of the control subactions, we execute it anyway
            if (subact.StartsWith("IfVar") || subact.StartsWith("Else") || subact.StartsWith("EndIf"))
                SubactionLoader.executeSubaction(subact, actor, this);
        }
    }
}

/*
public class TransitionState
{
    public int priority;

    public virtual void CheckTransition(AbstractFighter actor) { }
}


public class InputTransitions : TransitionState
{
    InputType input, direction;
    string neutral, tilt, smash;

    public InputTransitions(InputType _input, InputType _direction, string _neutral=null, string _tilt=null, string _smash = null)
    {
        input = _input;
        direction = _direction;
        neutral = _neutral;
        tilt = _tilt;
        smash = _smash;
    }
}
public class InputTransition : TransitionState
{
    InputType input;
    string action;
    public int priority = 0; //Resolve smashes, then direction, then inputs

    public InputTransition(InputType _input, string _action)
    {
        input = _input;
        action = _action;
    }

    public override void CheckTransition(AbstractFighter actor)
    {
        if (actor.KeyBuffered(input))
            actor.SendMessage("DoAction", action);
    }
}

public class DirectionTransition : TransitionState
{
    InputType direction;
    string action;
    bool smash;
    public int priority = 1; //Resolve smashes, then direction, then inputs

    public DirectionTransition(InputType _direction, string _action, bool _smash = false)
    {
        direction = _direction;
        action = _action;
        smash = _smash;
    }

    public override void CheckTransition(AbstractFighter actor)
    {
        if ((smash && actor.CheckSmash(direction)) || (!smash && (actor.KeyHeld(direction))))
            actor.SendMessage("DoAction", action);
    }
}

public class DirectionalInputTransition : TransitionState
{
    InputType direction;
    InputType input;
    string action;
    bool smash;
    public int priority = 2; //Resolve smashes, then direction, then inputs

    public DirectionalInputTransition(InputType _direction, InputType _input, string _action, bool _smash = false)
    {
        direction = _direction;
        input = _input;
        action = _action;
        smash = _smash;    
    }


    public override void CheckTransition(AbstractFighter actor)
    {
        //Check for either a smash or held
        if ((smash && actor.CheckSmash(direction)) || (!smash && (actor.KeyHeld(direction))))
        {
            if (actor.KeyBuffered(input))
                actor.SendMessage("DoAction", action);
        }
    }
}
*/