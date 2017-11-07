using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// HITBOX SUBACTION
/// Deactivates a given hitbox. If the hitbox is not active, this subaction does nothing.
/// 
/// Arguments:
///     name - required string - the name of the hitbox to deactivate
/// </summary>
public class SubactionDeactivateHitbox : Subaction
{
    public override void Execute(BattleObject obj, GameAction action)
    {
        string name = (string) GetArgument("name", obj, action);
        if (action.hitboxes.ContainsKey(name))
            action.hitboxes[name].Deactivate();
        else
            Debug.LogWarning("Current action has no hitbox names " + name);
    }

    public override SubactionCategory getCategory()
    {
        return SubactionCategory.HITBOX;
    }

    public override bool executeInBuilder()
    {
        return true;
    }

    public override void generateDefaultArguments()
    {
        arg_list.Add(new SubactionVarData("name", "constant", "string", "", false));
        BuildDict();
    }
}