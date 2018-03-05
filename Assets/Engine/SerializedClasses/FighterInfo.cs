using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using UnityEngine.U2D;

[System.Serializable]
public class FighterInfo {

    public string display_name;
    public string franchise_icon_path;
    public string css_icon_path;
    public string css_portrait_path;
    public string action_file_path;
    public string sound_path;

    public SpriteInfo sprite_info;
    public List<FighterPalette> colorPalettes;
    public List<VarData> variables;


    /// <summary>
    /// Directory name is not serialized, and is set when loaded so the program knows
    /// the folder name of the fighter, which might not match the fighter.
    /// </summary>
    [System.NonSerialized]
    public string directory_name;
    [System.NonSerialized]
    public Sprite franchise_icon_sprite;
    [System.NonSerialized]
    public Sprite css_icon_sprite;
    [System.NonSerialized]
    public Sprite css_portrait_sprite;
    [System.NonSerialized]
    public ActionFile action_file;
    [System.NonSerialized]
    public bool initialized = false;

    public void WriteJSON(string path)
    {
        string json = JsonUtility.ToJson(this, true);
        File.WriteAllText(path, json);
    }

    public void LoadDirectory(string directoryName)
    {
        directory_name = FileLoader.GetFighterPath(directoryName);
        franchise_icon_sprite = FileLoader.LoadSprite(FileLoader.PathCombine(directory_name,franchise_icon_path));
        css_icon_sprite = FileLoader.LoadSprite(FileLoader.PathCombine(directory_name, css_icon_path));
        css_portrait_sprite = FileLoader.LoadSprite(FileLoader.PathCombine(directory_name, css_portrait_path));
        //sprite_info.sprite_atlas = FileLoader.LoadSprite(FileLoader.PathCombine(directory_name, sprite_info.sprite_atlas_path);
        string action_file_json = FileLoader.LoadTextFile(FileLoader.PathCombine(directory_name, action_file_path));
        action_file = JsonUtility.FromJson<ActionFile>(action_file_json);
        initialized = true;
    }

    public static FighterInfo LoadFighterInfoFile(string directory, string filename="fighter_info.json")
    {
        string dir = FileLoader.GetFighterPath(directory);
        string combinedPath = Path.Combine(dir, filename);
        if (File.Exists(combinedPath))
        {
            string json = File.ReadAllText(combinedPath);
            FighterInfo info = JsonUtility.FromJson<FighterInfo>(json);
            info.LoadDirectory(directory);
            return info;
        }
        else
        {
            Debug.LogWarning("No fighter file found at " + directory + "/" + filename);
            return null;
        }
    }

    public VarData GetVarByName(string name)
    { 
        foreach (VarData data in variables)
        {
            if (data.name == name) return data;
        }
        return null;
    }

    public void CreateOrUpdateVarData(VarData newData)
    {
        foreach (VarData data in variables)
        {
            if (data.name == newData.name)
            {
                data.type = newData.type;
                data.value = newData.value;
                return;
            }
        }
        variables.Add(newData);
    }
}

[System.Serializable]
public class FighterPalette
{
    public int id;
    public string displayColor;
    public List<ColorMap> colorMappings;

    public static Color StringToColor(string colorString)
    {
        Color retColor = new Color();
        ColorUtility.TryParseHtmlString(colorString, out retColor);
        return retColor;
    }
}

[System.Serializable]
public class ColorMap
{
    public string from_color;
    public string to_color;
}

[System.Serializable]
public class SpriteInfo
{
    public string sprite_directory;
    public string sprite_prefix;
    public string sprite_default;
    public float  sprite_pixelsPerUnit;
    public string sprite_atlas_path;
    public SpriteAtlas sprite_atlas;
}