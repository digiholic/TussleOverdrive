using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomPropertyDrawer(typeof(DynamicAction))]
public class DynamicActionDrawer: PropertyDrawer
{
    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
        
        EditorGUI.PropertyField(position, property, label, true);
        if (property.isExpanded)
        {
            Color oldColor = GUI.color;
            GUI.color = Color.red;
            if (GUI.Button(new Rect(position.xMin + 30f, position.yMax - 20f, position.width - 30f, 20f), "delete"))
            {
                ActionFileLoader loader = (ActionFileLoader)property.serializedObject.targetObject;
                loader.DeleteAction(property.FindPropertyRelative("name").stringValue);
            }
            GUI.color = oldColor;
        }
    }

    public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
    {
        if (property.isExpanded)
            return EditorGUI.GetPropertyHeight(property) + 20f;
        return EditorGUI.GetPropertyHeight(property);
    }
}
