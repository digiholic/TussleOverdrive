using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Linq;

public class SubactionFactory {
    
    public static Subaction GetSubaction(string subactionName)
    {
        Type t = Type.GetType("Subaction"+subactionName);
        if (t != null) {
            object obj = Activator.CreateInstance(t);
            if (obj.GetType().IsSubclassOf(typeof(Subaction)))
            {
                Subaction sub = (Subaction) obj;
                sub.SubactionName = subactionName;
                return sub;
            }
            return null;
        }
        return null;
    }

    public static Subaction AddNewSubaction(string subactionName, SubActionGroup group)
    {
        Subaction sub = GetSubaction(subactionName);
        if (sub != null)
        {
            sub.generateDefaultArguments();
            group.subactions.Add(sub);
        }
        return sub;
    }

    public static Subaction LoadSubactionAs(Subaction subact, string subactionName)
    {
        Subaction sub = GetSubaction(subactionName);
        if (sub != null)
        {
            sub.CreateFromOther(subact);
        }
        return sub;
    }

    public static List<String> GetSubactionNamesByCategory(string cat)
    {
        switch (cat)
        {
            case "Control":
                return new List<String>()
                {
                    "SubactionDoAction",
                    "SubactionDoTransition",
                    "SubactionElse",
                    "SubactionEndIf",
                    "SubactionIfVar",
                    "SubactionSetAnchor",
                    "SubactionSetFrame",
                    "SubactionSetVar"
                };
            case "Behavior":
                return new List<String>()
                {
                    "SubactionChangeSpeed",
                    "SubactionShiftPosition",
                    "SubactionPlaySound"
                };
            case "Animation":
                return new List<String>()
                {
                    "SubactionChangeSprite",
                    "SubactionChangeSubimage",
                    "SubactionFlip",
                    "SubactionShiftSprite",
                    "SubactionRotate",
                    "SubactionUnrotate"
                };
            case "Hitbox":
                return new List<String>()
                {
                    "SubactionActivateHitbox",
                    "SubactionCreateHitbox",
                    "SubactionDeactivateHitbox",
                    "SubactionModifyHitbox"
                };
            default:
                return new List<String>();
        }
    }
}