using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// This class is essentially a fighter info, but as a monobehavior. This lets us set stuff in the editor for events, while keeping the serialized class seperate.
/// think of it kinda like a factory object but using the Unity editor.
/// </summary>
public class MonoFighter : MonoBehaviour
{
    public string display_name;
    public string franchise_icon_path;
    public string css_icon_path;
    public string css_portrait_path;
    public string action_file_path;
    public string sprite_info_path;
    public string sound_path;

    public List<FighterPalette> colorPalettes;
    public List<VarData> variables;

    public TextAsset json_file;

    public FighterInfo getFighterInfo()
    {
        FighterInfo info = new FighterInfo();
        info.displayName = display_name;
        info.franchiseIconPath = franchise_icon_path;
        info.cssIconPath = css_icon_path;
        info.cssPortraitPath = css_portrait_path;
        info.actionFilePath = action_file_path;
        info.spriteInfoPath = sprite_info_path;
        info.soundPath = sound_path;

        info.colorPalettes = colorPalettes;
        info.variables = variables;

        return info;
    }

    public void fromFighterInfo(FighterInfo info)
    {
        display_name = info.displayName;
        franchise_icon_path = info.franchiseIconPath;
        css_icon_path = info.cssIconPath;
        css_portrait_path = info.cssPortraitPath;
        action_file_path = info.actionFilePath;
        sprite_info_path = info.spriteInfoPath;
        sound_path = info.soundPath;

        colorPalettes = info.colorPalettes;
        variables = info.variables;
    }

    void Awake()
    {
        if (json_file != null)
        {
            LoadFighter();
        }
    }

    public void LoadFighter()
    {
        FighterInfo fighter_info = JsonUtility.FromJson<FighterInfo>(json_file.text);
        fromFighterInfo(fighter_info);
    }
}
