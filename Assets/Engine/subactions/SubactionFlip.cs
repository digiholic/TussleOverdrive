using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// ANIMATION SUBACTION
/// Flips the sprite. No arguments required.
/// </summary>
public class SubactionFlip : Subaction {

    public override void Execute(BattleObject actor, GameAction action)
    {
        base.Execute(actor, action);
        actor.SendMessage("flip");
    }

    public override SubactionType getSubactionType()
    {
        return SubactionType.ANIMATION;
    }
}
