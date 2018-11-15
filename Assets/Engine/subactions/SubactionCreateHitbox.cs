using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// HITBOX SUBACTION
/// Creates a hitbox with all of the given parameters. Other than the required name,
/// every single argument on this gets passed to the hitbox.
/// 
/// Arguments
///     name - required string - the name of the hitbox, which is used to reference it in later subactions
///     ...
///     Any number of other arguments are passed to the hitbox. Hitboxes have many possible parameters, please consult Hitbox documentation for a list.
/// </summary>
public class SubactionCreateHitbox : Subaction {
    public override void Execute(BattleObject actor, GameAction action)
    {
        string name = "";
        Dictionary<string, string> hbox_dict = new Dictionary<string, string>();
        foreach (SubactionVarData data in arg_dict.Values)
        {
            if (data.name == "name")
                name = (string)data.GetData(actor, action);
            else
            {
                hbox_dict.Add(data.name, (string)data.GetData(actor, action));
            }
        }
        if (name != "")
        {
            Hitbox hbox = HitboxLoader.loader.LoadHitbox(actor.GetAbstractFighter(), action, hbox_dict);
            action.hitboxes.Add(name, hbox);
        }
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
        BuildDict();
    }
    */
}
