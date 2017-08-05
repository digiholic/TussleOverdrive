using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

//[CustomPropertyDrawer(typeof(ActionGroup))]
public class ActionGroupDrawer : PropertyDrawer
{
    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
        Rect contentPosition = EditorGUI.PrefixLabel(position, label);
        EditorGUI.PropertyField(position, property.FindPropertyRelative("subactions"), GUIContent.none,true);
    }
}
