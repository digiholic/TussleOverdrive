using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// HITBOX SUBACTION
/// Activates a given hitbox for a given amount of time. If life is set to -1,
/// hitbox will remain active until manually deactivated.
/// 
/// Arguments:
///     name - required string - The hitbox to activate
///     life - optional int (default -1) - The time the hitbox is active. If set to -1, hitbox will be active until manually closed.
/// </summary>
public class SubactionActivateHitbox : Subaction
{
    public override void Execute(BattleObject obj, GameAction action)
    {
        base.Execute(obj, action);
        string name = (string)GetArgument("hitboxName", obj, action);
        int life = (int)GetArgument("life", obj, action, -1);
        
        if (action.hitboxes.ContainsKey(name))
            action.hitboxes[name].Activate(life);
        else
            Debug.LogWarning("Current action has no hitbox named " + name);
    }

    public override SubactionType getSubactionType()
    {
        return SubactionType.HITBOX;
    }

    public override bool canExecuteInBuilder()
    {
        return true;
    }

    /*
    public override void generateDefaultArguments()
    {
        arg_list.Add(new SubactionVarData("name", "constant", "string", "", false));
        arg_list.Add(new SubactionVarData("life", "constant", "int", "-1", false));
        BuildDict();
    }
    */
}