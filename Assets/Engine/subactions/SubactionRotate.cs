using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// ANIMATION SUBACTION
/// Rotates the fighter by a given number of degrees.
/// 
/// Arguments:
///     degrees - required float - the degrees to rotate by
/// </summary>
public class SubactionRotate : Subaction {

    public override void Execute(BattleObject actor, GameAction action)
    {
        base.Execute(actor, action);
        float degrees = (float) GetArgument("degrees", actor, action);
        actor.GetSpriteHandler().RotateSprite(degrees);
    }

    public override SubactionCategory getCategory()
    {
        return SubactionCategory.ANIMATION;
    }

    public override void generateDefaultArguments()
    {
        arg_list.Add(new SubactionVarData("degrees", "constant", "float", "", false));
        BuildDict();
    }
}
