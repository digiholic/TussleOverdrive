using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(Settings))]
public class SettingsEditor : Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

        Settings setting_object = (Settings)target;
        if (GUILayout.Button("Load"))
        {
            setting_object.LoadSettings();
        }
        if (GUILayout.Button("Save"))
        {
            setting_object.SaveSettings();
        }
    }
}