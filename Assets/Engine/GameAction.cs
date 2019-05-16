using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class GameAction {
    public string name;
    public string sprite_name;
    public int sprite_rate = 1;
    public bool loop = false;
    public int length = 1;
    public string exit_action = "NeutralAction";

    public int current_frame;

    public SubDict subactionCategories = new SubDict();

    public Dictionary<string, Hitbox> hitboxes = new Dictionary<string, Hitbox>();
    public Dictionary<string, HitboxLock> hitbox_locks = new Dictionary<string, HitboxLock>();

    public List<bool> cond_list = new List<bool> { true };
    public int cond_depth = 0;

    public Dictionary<string, BattleObjectVarData> variables = new Dictionary<string, BattleObjectVarData>();
    public Dictionary<string, object> variables_to_pass = new Dictionary<string, object>();

    protected int last_frame;
    protected BattleObject actor;
    protected BattleController game_controller;
    protected bool isInBuilder = false;

    public void SetDynamicAction(DynamicAction dynAction)
    {
        if (dynAction == null)
        {
            UnityEngine.Debug.LogWarning("Dynamic Action Null. Skipping SetDynamicAction");
            return;
        }
        name = dynAction.name;
        length = dynAction.length;
        sprite_name = dynAction.sprite;
        sprite_rate = dynAction.sprite_rate;
        loop = dynAction.loop;
        exit_action = dynAction.exit_action;
        
        if (dynAction.subactionCategories == null)
        {
            UnityEngine.Debug.LogWarning("Dynamic Action subactions are null. This should be an empty dict instead.");
            return;
        }
        foreach (KeyValuePair<string, List<SubactionData>> keyVal in dynAction.subactionCategories)
        {
            List<Subaction> categoryList = new List<Subaction>();
            foreach (SubactionData data in keyVal.Value)
            {
                categoryList.Add(SubactionFactory.GenerateSubactionFromData(data));
            }
            subactionCategories.Add(keyVal.Key, categoryList);
        }
    }

    public virtual void SetUp (BattleObject obj) {
        last_frame = length;
        actor = obj;
        actor.BroadcastMessage("ChangeSprite",sprite_name,SendMessageOptions.DontRequireReceiver);
        game_controller = BattleController.current_battle;
        foreach (Subaction subaction in subactionCategories.GetIfKeyExists(SubactionGroup.SETUP))
        {
            CheckCondAndExecute(subaction);
        }
    }

    // Update is called once per frame
    public virtual void Update() {
        if (sprite_rate != 0) // if it's zero, no need to animate
        {
            int sprite_number = Mathf.FloorToInt(current_frame / sprite_rate);
            if (sprite_rate < 0)
                sprite_number = Mathf.FloorToInt(current_frame / sprite_rate) - 1;
            if (loop)
                actor.SendMessage("ChangeSubimageWithLoop", sprite_number,SendMessageOptions.RequireReceiver);
            else
                actor.SendMessage("ChangeSubimage", sprite_number,SendMessageOptions.RequireReceiver);
        }

        foreach (Subaction subaction in subactionCategories.GetIfKeyExists(SubactionGroup.ONFRAME(current_frame)))
            CheckCondAndExecute(subaction);
        if (current_frame >= last_frame)
            if (exit_action != null && exit_action != "")
                actor.SendMessage("DoAction", exit_action);
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
        foreach (Subaction subaction in subactionCategories.GetIfKeyExists(SubactionGroup.TEARDOWN))
            CheckCondAndExecute(subaction);
    }

    public virtual void stateTransitions()
    {
        foreach (Subaction subaction in subactionCategories.GetIfKeyExists(SubactionGroup.STATETRANSITION))
            CheckCondAndExecute(subaction);
    }
    
    public void ChangeFrame(int frame, bool relative)
    {
        if (relative) current_frame += frame;
        else current_frame = frame;
    }

    public void AdjustLength(int new_length)
    {
        last_frame = new_length;
    }

    //Initialize the variable if it's not set yet, then return it
    public BattleObjectVarData GetVar(string var_name)
    {
        if (!HasVar(var_name))
        {
            UnityEngine.Debug.Log("Attempting to get variable without setting one: " + var_name);
            //return new BattleObjectVarData(var_name, null);
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

    public int GetIntVar(string var_name)
    {
        return GetVar(var_name).GetIntData();
    }

    public float GetFloatVar(string var_name)
    {
        return GetVar(var_name).GetFloatData();
    }

    public bool GetBoolVar(string var_name)
    {
        return GetVar(var_name).GetBoolData();
    }

    public string GetStringVar(string var_name)
    {
        return GetVar(var_name).GetStringData();
    }

    public void PassVariable(string var_name, object var_value)
    {
        variables_to_pass[var_name] = var_value;
    }

    public void setIsInBuilder(bool builderFlag)
    {
        isInBuilder = builderFlag;
    }

    private void CheckCondAndExecute(Subaction subact)
    {
        if (!cond_list.Contains(false)) //If there are no falses in the list, execute the action
        {
            //This will only execute the subaction if we aren't in the builder, or if this one will always execute
            if (!isInBuilder || subact.canExecuteInBuilder())
            {
                subact.Execute(actor, this);
            }
        }
        else
        {
            //Check if the subaction is one of the control subactions, we execute it anyway
            if (subact.isConditional())
                subact.Execute(actor, this);
        }
    }
}

[System.Serializable]
public class SubDict : SerializableDictionary<string, List<Subaction>, SubactionListStorage> {
    /// <summary>
    /// If the key is in the dict, get the list it points to. Create an empty list otherwise.
    /// Just a quick convenience method, since there'll be a key for every frame
    /// Empty lists should be cleaned up before serializing to save space
    /// </summary>
    /// <param name="key">The dict key to search for</param>
    /// <returns>The list from the dict if the key exists, empty list otherwise</returns>
    public List<Subaction> GetIfKeyExists(string key)
    {
        if (ContainsKey(key))
        {
            return this[key];
        }
        else
        {
            this[key] = new List<Subaction>();
            return this[key];
        }

    }
}

[System.Serializable]
public class SubactionListStorage : SerializableDictionary.Storage<List<Subaction>> { }