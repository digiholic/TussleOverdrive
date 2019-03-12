using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(BanishablePanel))]
public class PanelBanisherButtons : Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

        BanishablePanel banisher = (BanishablePanel)target;
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