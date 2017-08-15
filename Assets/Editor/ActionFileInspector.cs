using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(ActionFileLoader))]
public class ActionFileInspector : Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

        ActionFileLoader info = (ActionFileLoader)target;
        if (GUILayout.Button("Load"))
        {
            info.LoadActions();
        }
        if (GUILayout.Button("Save"))
        {
            info.SaveActions();
        }
    }
}