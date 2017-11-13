using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class SubactionFactory {
    
    public static Subaction GetSubaction(string subactionName)
    {
        Type t = Type.GetType("Subaction"+subactionName);
        if (t != null) {
            object obj = Activator.CreateInstance(t);
            if (obj.GetType().IsSubclassOf(typeof(Subaction)))
            {
                Subaction sub = (Subaction) obj;
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
}