using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// ANIMATION SUBACTION
/// Changes the sprite to the given sprite.
/// 
/// Arguments:
///     sprite - required string - the sprite to change to
/// </summary>
public class SubactionChangeSprite : Subaction
{
    public override void Execute(BattleObject obj, GameAction action)
    {
        string sprite = (string)GetArgument("sprite", obj, action);
        obj.GetSpriteHandler().ChangeSprite(sprite);
    }

    public override SubactionType getSubactionType()
    {
        return SubactionType.ANIMATION;
    }

    public override bool canExecuteInBuilder()
    {
        return true;
    }

    /*
    public override void generateDefaultArguments()
    {
        arg_list.Add(new SubactionVarData("sprite", "constant", "string", "", false));
        BuildDict();
    }
    */
}