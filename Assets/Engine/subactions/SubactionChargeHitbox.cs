using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// HITBOX SUBACTION
/// Increase the power of a hitbox based on its charge values
/// 
/// Arguments:
///     hitboxName - required string - The hitbox to charge
/// </summary>
public class SubactionChargeHitbox : Subaction
{
    public override void Execute(BattleObject obj, GameAction action)
    {
        base.Execute(obj, action);
        string name = (string)GetArgument("hitboxName", obj, action);
        
        if (action.hitboxes.ContainsKey(name))
            action.hitboxes[name].Charge();
        else
            Debug.LogWarning("Current action has no hitbox named " + name);
    }

    public override SubactionType getSubactionType()
    {
        return SubactionType.HITBOX;
    }

    public override bool canExecuteInBuilder()
    {
        return false;
    }
}
