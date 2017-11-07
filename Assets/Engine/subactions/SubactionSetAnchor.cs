using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// CONTROL SUBACTION
/// Sets a given Anchorpoint on the calling object
/// 
/// Arguments:
///     name - required string - the name of the anchor point to set. If it doesn't exist, it'll be made.
///     centerx - required int - the center x position (in pixel coordinates) of the anchor point. This is offset from the center of the sprite
///     centery - required int - the center y position (in pixel coordinates) of the anchor point. This is offset from the center of the sprite
/// </summary>
public class SubactionSetAnchor : Subaction
{
    public override void Execute(BattleObject obj, GameAction action)
    {
        base.Execute(obj, action);
        string name = (string) GetArgument("name", obj, action);
        int centerx = (int) GetArgument("centerx", obj, action);
        int centery = (int)GetArgument("centery", obj, action);
        obj.GetAnchorPoint(name).MoveAnchorPixel(centerx, centery);
    }

    public override SubactionCategory getCategory()
    {
        return SubactionCategory.CONTROL;
    }

    public override void generateDefaultArguments()
    {
        arg_list.Add(new SubactionVarData("name", "constant", "string", "", false));
        arg_list.Add(new SubactionVarData("centerx", "constant", "int", "", false));
        arg_list.Add(new SubactionVarData("centery", "constant", "int", "", false));
        BuildDict();
    }
}