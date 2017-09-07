using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

[System.Serializable]
public class StageInfo{
    public string stage_icon_path;
    public string stage_portrait_path;

    public string stage_name;

    [System.NonSerialized]
    public string directory_name;
    [System.NonSerialized]
    public Sprite stage_icon;
    [System.NonSerialized]
    public Sprite stage_portrait;
    [System.NonSerialized]
    public bool initialized = false;

    public void WriteJSON(string path)
    {
        string json = JsonUtility.ToJson(this, true);
        File.WriteAllText(path, json);
    }

    public void LoadDirectory(string directoryName)
    {
        directory_name = FileLoader.GetStagePath(directoryName);
        stage_icon = FileLoader.LoadSprite(FileLoader.PathCombine(directory_name, stage_icon_path));
        stage_portrait = FileLoader.LoadSprite(FileLoader.PathCombine(directory_name, stage_portrait_path));
        initialized = true;
    }

    public static StageInfo LoadStageInfoFile(string directory, string filename = "stage_info.json")
    {
        string dir = Path.Combine(FileLoader.StageDir.FullName, directory);
        string combinedPath = Path.Combine(dir, filename);
        if (File.Exists(combinedPath))
        {
            string json = File.ReadAllText(combinedPath);
            StageInfo info = JsonUtility.FromJson<StageInfo>(json);
            info.LoadDirectory(directory);
            return info;
        }
        else
        {
            Debug.LogWarning("No stage file found at " + directory + "/" + filename);
            return null;
        }
    }
}
