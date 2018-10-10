using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SubactionUtilities {
    public static string TypeToString(SubactionType subType)
    {
        switch (subType)
        {
            case SubactionType.CONTROL:
                return "Control";
            case SubactionType.BEHAVIOR:
                return "Behavior";
            case SubactionType.ANIMATION:
                return "Animation";
            case SubactionType.HITBOX:
                return "Hitbox";
            case SubactionType.OTHER:
                return "Other";
            default:
                throw new System.NotImplementedException("Subaction Type not recognized. Please add string to SubactionUtilities. "+subType);
        }
    }
}
