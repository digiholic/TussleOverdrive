using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;

/// <summary>
/// Creates a LegacyEditor action to modify a property in LegacyEditorData.
/// Uses reflection, so you'll need to initialize it with a property name and a value to set it to.
/// Will remember the previous value of the changed data, as well as adjacent properties
/// </summary>
public class ModifyLegacyEditorDataAction : LegacyEditorAction
{
    private string propertyToModify;
    private object propertyToModifyNextValue;
    private object propertyToModifyPreviousValue;
    private Dictionary<string, object> propertiesAlsoChangedValues = new Dictionary<string, object>();
    private Dictionary<string, object> propertiesAlsoChangedPreviousValues = new Dictionary<string, object>();

    private bool deselect = false;
    private object deselectValue = null;

    public void init(string prop, object val)
    {
        propertyToModify = prop;
        propertyToModifyNextValue = val;
    }

    public void enableDeselect(object val = null)
    {
        deselect = true;
        deselectValue = val;
    }

    public void addAdjacentProperty(string adjacentProp, object adjacentVal)
    {
        propertiesAlsoChangedValues[adjacentProp] = adjacentVal;
        //Set this just so there's something to look at in debug if we need to see what's being set. It should never be read before being set
        propertiesAlsoChangedPreviousValues[adjacentProp] = null;
    }
    /// <summary>
    /// Executes the action. This will get the property for the main data to modify, set the previous for undoing later, then change 
    /// Then it iterates through adjacent properties and sets them too.
    /// </summary>
    public override void execute()
    {
        //Get the previous value of the property, then change it
        PropertyInfo prop = LegacyEditorData.instance.GetType().GetProperty(propertyToModify);
        propertyToModifyPreviousValue = prop.GetValue(LegacyEditorData.instance);
        object valueToSet = propertyToModifyNextValue;
        if (deselect && (propertyToModifyPreviousValue == propertyToModifyNextValue))
        {
            valueToSet = deselectValue;
        }
        prop.SetValue(LegacyEditorData.instance, valueToSet);

        //Iterate through adjacent properties, set their previous values, then change them
        foreach(KeyValuePair<string,object> adjacentPropData in propertiesAlsoChangedValues)
        {
            PropertyInfo adjacentProp = LegacyEditorData.instance.GetType().GetProperty(adjacentPropData.Key);
            propertiesAlsoChangedPreviousValues[adjacentPropData.Key] = adjacentProp.GetValue(LegacyEditorData.instance);
            adjacentProp.SetValue(LegacyEditorData.instance, adjacentPropData.Value);
        }
    }

    /// <summary>
    /// Undo the action, reverting the property and all adjacent properties to their original values.
    /// </summary>
    public override void undo()
    {
        //Revert the property to it's previous value
        PropertyInfo prop = LegacyEditorData.instance.GetType().GetProperty(propertyToModify);
        prop.SetValue(LegacyEditorData.instance, propertyToModifyPreviousValue);

        //Iterate through adjacent properties, and revert them
        foreach (KeyValuePair<string, object> adjacentPropData in propertiesAlsoChangedPreviousValues)
        {
            PropertyInfo adjacentProp = LegacyEditorData.instance.GetType().GetProperty(adjacentPropData.Key);
            adjacentProp.SetValue(LegacyEditorData.instance, adjacentPropData.Value);
        }
    }

}
