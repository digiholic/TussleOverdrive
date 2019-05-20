using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CreateHitboxEditContextPanel : EditSubactionContextPanel
{
    public HitboxEditorCategory selectedCategory;
    public SubactionData hitboxData;


    public enum HitboxEditorCategory
    {
        PROPERTIES,
        DAMAGE,
        CHARGE,
        ADVANCED
    }

    public static HitboxEditorCategory CategoryFromString(string s)
    {
        if (s.Equals("Damage")) return HitboxEditorCategory.DAMAGE;
        else if (s.Equals("Charge")) return HitboxEditorCategory.CHARGE;
        else return HitboxEditorCategory.PROPERTIES;
    }

    public static string StringFromCategory(HitboxEditorCategory cat)
    {
        if (cat == HitboxEditorCategory.DAMAGE) return "Damage";
        else if (cat == HitboxEditorCategory.CHARGE) return "Charge";
        else return "Properties";
    }
}
