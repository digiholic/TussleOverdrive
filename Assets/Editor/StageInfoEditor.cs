using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(StageInfoLoader))]
public class StageInfoEditor : Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

        StageInfoLoader info = (StageInfoLoader)target;
        if (GUILayout.Button("Load"))
        {
            info.LoadStage();
        }
        if (GUILayout.Button("Save"))
        {
            info.SaveStage();
        }
    }
}