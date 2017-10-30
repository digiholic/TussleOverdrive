using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// CONTROL SUBACTION
/// Plays the given sound effect
/// 
/// Arguments:
///     name - required string - the name of the sound effect to play
/// </summary>
public class SubactionPlaySound : Subaction {

    public override void Execute(BattleObject actor, GameAction action)
    {
        base.Execute(actor, action);
        string name = (string) GetArgument("name", actor, action);
        actor.GetAbstractFighter().PlaySound(name);
    }

    public override SubactionCategory getCategory()
    {
        return SubactionCategory.CONTROL;
    }
}
