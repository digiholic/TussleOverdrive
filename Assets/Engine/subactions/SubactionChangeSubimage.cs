using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// ANIMATION SUBACTION
/// Changes to the given frame of animation
/// 
/// Arguments:
///     subimage - required int - the subimage index of the animation to switch to
/// </summary>
public class SubactionChangeSubimage : Subaction {
    public override void Execute(BattleObject obj, GameAction action)
    {
        int subimage = (int) GetArgument("subimage", obj, action);
        obj.GetSpriteHandler().ChangeSubimage(subimage);
    }

    public override SubactionType getSubactionType()
    {
        return SubactionType.ANIMATION;
    }

    public override bool executeInBuilder()
    {
        return true;
    }

    /*
    public override void generateDefaultArguments()
    {
        arg_list.Add(new SubactionVarData("subimage", "constant", "int", "", false));
        BuildDict();
    }
    */
}
