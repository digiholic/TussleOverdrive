using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using UnityEditor;

public class FighterInfoLoader : MonoBehaviour {
    public FighterInfo fighter_info = new FighterInfo();
    public string directory;
    public string filename;

    public TextAsset json_file;

    private DirectoryInfo fightersDirectory = new DirectoryInfo("Assets/Resources/Fighters");

    public void SaveFighter()
    {
        string dir = Path.Combine(fightersDirectory.FullName, directory);
        fighter_info.WriteJSON(Path.Combine(dir, filename));
    }

    public void LoadFighter()
    {
        if (json_file != null)
        {
            fighter_info = JsonUtility.FromJson<FighterInfo>(json_file.text);
        }
        else
        {
            string dir = Path.Combine(fightersDirectory.FullName, directory);
            string combinedPath = Path.Combine(dir, filename);
            if (File.Exists(combinedPath))
            {
                string json = File.ReadAllText(combinedPath);
                fighter_info = JsonUtility.FromJson<FighterInfo>(json);
            }
        }
    }
}

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