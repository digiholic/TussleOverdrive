using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class FighterInfoLoader : MonoBehaviour {
    [SerializeField]
    private FighterInfo fighter_info = new FighterInfo();

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
            fighter_info = FighterInfo.LoadFighterInfoFile(directory, filename);
        }
        fighter_info.LoadDirectory(directory);
    }

    public FighterInfo GetFighterInfo()
    {
        if (fighter_info.initialized) return fighter_info;
        else
        {
            fighter_info.LoadDirectory(directory);
            return fighter_info;
        }
    }

    public void SetFighterInfo(FighterInfo info)
    {
        fighter_info = info;
        directory = info.directory_name;
        filename = "fighter_info.json";
    }
}