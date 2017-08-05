using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomPropertyDrawer(typeof(VarData))]
public class VarDataDrawer : PropertyDrawer
{
    public override void OnGUI(Rect contentPosition, SerializedProperty property, GUIContent label)
    {
        //Rect contentPosition = EditorGUI.PrefixLabel(position, label);
        Rect leftRect = new Rect(contentPosition.x, contentPosition.y, (contentPosition.width / 2)*0.8f, contentPosition.height);
        Rect rightRect = new Rect(contentPosition.x + contentPosition.width / 2, contentPosition.y, contentPosition.width / 2, contentPosition.height);
        EditorGUI.PropertyField(leftRect, property.FindPropertyRelative("name"),GUIContent.none);
        EditorGUI.indentLevel = 0;
        EditorGUI.PropertyField(rightRect, property.FindPropertyRelative("value"), GUIContent.none);
    }
}
