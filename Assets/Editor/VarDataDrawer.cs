using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

// [CustomPropertyDrawer(typeof(VarData))]
// public class VarDataDrawer : PropertyDrawer
// {
//     public override void OnGUI(Rect contentPosition, SerializedProperty property, GUIContent label)
//     {
//         //Rect contentPosition = EditorGUI.PrefixLabel(position, label);
//         Rect leftRect = new Rect(contentPosition.x, contentPosition.y, (contentPosition.width / 3), contentPosition.height);
//         Rect centerRect = new Rect(contentPosition.x + contentPosition.width / 3, contentPosition.y, contentPosition.width / 3, contentPosition.height);
//         Rect rightRect = new Rect(contentPosition.x + 2*(contentPosition.width / 3), contentPosition.y, contentPosition.width / 3, contentPosition.height);

//         Debug.Log(property.type);
//         EditorGUI.PropertyField(leftRect, property.FindPropertyRelative("name"),GUIContent.none);
//         EditorGUI.indentLevel = 0;
//         EditorGUI.PropertyField(centerRect, property.FindPropertyRelative("value"), GUIContent.none);
//         EditorGUI.PropertyField(rightRect, property.FindPropertyRelative("type"), GUIContent.none);
//     }
// }
