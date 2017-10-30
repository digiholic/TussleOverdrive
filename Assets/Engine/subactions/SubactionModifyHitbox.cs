using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// HITBOX SUBACTION
/// Modifies a hitbox with the given name to have the given data.
/// 
/// Arguments
///     name - required string - the name of the hitbox to modify
///     ...
///     Any number of other arguments are passed to the hitbox. Hitboxes have many possible parameters, please consult Hitbox documentation for a list.
/// </summary>
public class SubactionModifyHitbox : Subaction
{
    public override void Execute(BattleObject actor, GameAction action)
    {
        string name = "";
        Dictionary<string, string> hbox_dict = new Dictionary<string, string>();
        foreach (SubactionVarData data in arg_list)
        {
            if (data.name == "name")
                name = (string)data.GetData(actor, action);
            else
            {
                hbox_dict.Add(data.name, (string)data.GetData(actor, action));
            }
        }
        if (name != "" && action.hitboxes.ContainsKey(name))
        {
            action.hitboxes[name].LoadValuesFromDict(actor.GetAbstractFighter(), hbox_dict);
        }
    }

    public override SubactionCategory getCategory()
    {
        return SubactionCategory.HITBOX;
    }

    public override bool executeInBuilder()
    {
        return true;
    }
}
