using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.IO;

public class ActionFileLoader : MonoBehaviour {
    public ActionFile action_file = new ActionFile();
    public string directory;
    public string filename;

    public TextAsset json_file;

    private DirectoryInfo fightersDirectory = new DirectoryInfo("Assets/Resources/Fighters");

    public void SaveActions()
    {
        string dir = Path.Combine(fightersDirectory.FullName, directory);
        action_file.WriteJSON(Path.Combine(dir, filename));
    }

    public void LoadActions()
    {
        if (json_file != null)
        {
            action_file = JsonUtility.FromJson<ActionFile>(json_file.text);
        }
        else
        {
            string dir = Path.Combine(fightersDirectory.FullName, directory);
            string combinedPath = Path.Combine(dir, filename);
            if (File.Exists(combinedPath))
            {
                string json = File.ReadAllText(combinedPath);
                action_file = JsonUtility.FromJson<ActionFile>(json);
            }
        }
    }
}

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