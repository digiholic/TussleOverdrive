using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using UnityEditor;

public class StageInfoLoader : MonoBehaviour
{
    [SerializeField]
    private StageInfo stage_info = new StageInfo();

    public string directory;
    public string filename;

    public TextAsset json_file;

    private DirectoryInfo stageDirectory = new DirectoryInfo("Assets/Resources/Stages");

    public void SaveStage()
    {
        string dir = Path.Combine(stageDirectory.FullName, directory);
        stage_info.WriteJSON(Path.Combine(dir, filename));
    }

    public void LoadStage()
    {
        if (json_file != null)
        {
            stage_info = JsonUtility.FromJson<StageInfo>(json_file.text);
        }
        else
        {
            stage_info = StageInfo.LoadStageInfoFile(directory, filename);
        }
        stage_info.LoadDirectory(directory);
    }

    public StageInfo GetFighterInfo()
    {
        if (stage_info.initialized) return stage_info;
        else
        {
            stage_info.LoadDirectory(directory);
            return stage_info;
        }
    }

    public void SetFighterInfo(StageInfo info)
    {
        stage_info = info;
        directory = info.directory_name;
        filename = "fighter_info.json";
    }
}

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