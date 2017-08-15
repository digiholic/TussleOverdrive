using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(ActionHandler))]
public class ActionHandlerEditor : Editor {

    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

        ActionHandler handler = (ActionHandler)target;

        string actionName = "";
        string current_frame = "";

        if (handler.CurrentAction != null)
        {
            actionName = handler.CurrentAction.name;
            current_frame = handler.CurrentAction.current_frame.ToString();
        }

        GUILayout.TextField(actionName);
        GUILayout.TextField(current_frame);
    }
}
