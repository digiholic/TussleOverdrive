using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Diagnostics;
using System;

public class GameAction {
    private StackTrace stackTrace = new StackTrace();

    public string name;
    public string sprite_name;
    public int sprite_rate = 1;
    public bool loop = false;
    public int length = 1;
    public string exit_action = "NeutralAction";

    public int current_frame;

    public SubActionGroup set_up_subactions = new SubActionGroup();
    public SubActionGroup state_transition_subactions = new SubActionGroup();
    public SubActionGroup subactions_on_frame = new SubActionGroup();
    public SubActionGroup tear_down_subactions = new SubActionGroup();

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
        foreach (Subaction subaction in set_up_subactions.subactions)
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

        foreach (Subaction subaction in subactions_on_frame.subactions)
            CheckCondAndExecute(subaction);
        if (current_frame >= last_frame)
            if (exit_action != null && exit_action != "")
                actor.SendMessage("DoAction", exit_action);

        current_frame++;
    }

    public virtual void TearDown(GameAction new_action)
    {
        //Pass the variables we've set to pass to the next action
        foreach (KeyValuePair<string,object> passvar in variables_to_pass)
        {
            new_action.SetVar(passvar.Key, passvar.Value);
        }

        //Release all hitbox locks
        foreach (HitboxLock hlock in hitbox_locks.Values)
        {
            hlock.Destroy();
        }

        //Deactivate and destroy hitboxes at the end of the action
        foreach (Hitbox hbox in hitboxes.Values)
        {
            hbox.Deactivate();
            GameObject.Destroy(hbox.gameObject);
        }
        foreach (Subaction subaction in tear_down_subactions.subactions)
            CheckCondAndExecute(subaction);
    }

    public virtual void stateTransitions()
    {
        foreach (Subaction subaction in state_transition_subactions.subactions)
            CheckCondAndExecute(subaction);
    }
    
    public void SetDynamicAction(DynamicAction dynAction)
    {
        name = dynAction.name;
        length = dynAction.length;
        sprite_name = dynAction.sprite;
        sprite_rate = dynAction.sprite_rate;
        loop = dynAction.loop;
        exit_action = dynAction.exit_action;

        set_up_subactions = dynAction.set_up_subactions;
        state_transition_subactions = dynAction.state_transition_subactions;
        tear_down_subactions = dynAction.tear_down_subactions;
        subactions_on_frame = dynAction.subactions_on_frame;
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

    private void CheckCondAndExecute(Subaction subact)
    {
        if (!cond_list.Contains(false)) //If there are no falses in the list, execute the action
        {
            subact.Execute(actor, this);
        }
        else
        {
            //Check if the subaction is one of the control subactions, we execute it anyway
            if (subact.isConditional())
                subact.Execute(actor, this);
        }
    }
}