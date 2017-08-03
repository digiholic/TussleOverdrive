using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

[System.Serializable]
public class FighterInfo {

    public string display_name;
    public string franchise_icon_path;
    public string css_icon_path;
    public string css_portrait_path;
    public List<FighterPalette> colorPalettes;
    public List<VarData> variables;
    public string action_file_path;

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

    public void WriteJSON(string path)
    {
        string json = JsonUtility.ToJson(this, true);
        File.WriteAllText(path, json);
    }

    public void LoadDirectory(string directoryName)
    {
        directory_name = directoryName;
        franchise_icon_sprite = Resources.Load<Sprite>("Fighters/" + directory_name + "/" + franchise_icon_path);
        css_icon_sprite = Resources.Load<Sprite>("Fighters/" + directory_name + "/" + css_icon_path);
        css_portrait_sprite = Resources.Load<Sprite>("Fighters/" + directory_name + "/" + css_portrait_path);
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
public class VarData
{
    public string name;
    public string value;
    public VarType type;
}

public enum VarType
{
    INT,
    STRING,
    FLOAT,
    BOOL
}