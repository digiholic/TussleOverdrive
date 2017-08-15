using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(FighterInfoLoader))]
public class FighterInfoEditor : Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

        FighterInfoLoader info = (FighterInfoLoader)target;
        if (GUILayout.Button("Load"))
        {
            info.LoadFighter();
        }
        if (GUILayout.Button("Save"))
        {
            info.SaveFighter();
        }
    }
}
