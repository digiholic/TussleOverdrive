using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActionFileLoader : MonoBehaviour {
    public ActionFile action_file = new ActionFile();
    public string directory;
    public string filename;

    public TextAsset json_file;

    public void SaveActions()
    {
        Debug.Log(FileLoader.GetFighterPath(directory));
        action_file.WriteJSON(FileLoader.PathCombine(FileLoader.GetFighterPath(directory), filename));
    }

    public void LoadActions()
    {
        if (json_file != null)
        {
            action_file = JsonUtility.FromJson<ActionFile>(json_file.text);
        }
        else
        {
            string combinedPath = FileLoader.PathCombine(FileLoader.GetFighterPath(directory),filename);
            string json = FileLoader.LoadTextFile(combinedPath);
            action_file = JsonUtility.FromJson<ActionFile>(json);
        }
    }

    public void DeleteAction(string action_name)
    {
        action_file.DeleteByName(action_name);
    }
}