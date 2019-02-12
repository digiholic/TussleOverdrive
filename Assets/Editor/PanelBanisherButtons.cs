using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(StartsBanished))]
public class PanelBanisherButtons : Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

        StartsBanished banisher = (StartsBanished)target;
        if (GUILayout.Button("Banish"))
        {
            banisher.Banish();
        }
        if (GUILayout.Button("Unbanish"))
        {
            banisher.Unbanish();
        }
    }
}