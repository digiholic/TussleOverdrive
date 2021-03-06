﻿using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;

public class ModifyImageDefEditorAction : LegacyEditorAction
{
    private string propertyToModify;
    private object propertyToModifyNextValue;
    private object propertyToModifyPreviousValue;

    public void init(string prop, object val)
    {
        propertyToModify = prop;
        propertyToModifyNextValue = val;
    }

    public override void execute()
    {
        ImageDefinition def = LegacyEditorData.instance.currentImageDef;

        //Get the previous value of the property, then change it
        PropertyInfo prop = def.GetType().GetProperty(propertyToModify);
        propertyToModifyPreviousValue = prop.GetValue(def);
        
        object valueToSet = propertyToModifyNextValue;
        prop.SetValue(def, valueToSet);
    }

    public override void undo()
    {
        ImageDefinition def = LegacyEditorData.instance.currentImageDef;

        //Revert the property to it's previous value
        PropertyInfo prop = def.GetType().GetProperty(propertyToModify);
        prop.SetValue(def, propertyToModifyPreviousValue);
    }
}
