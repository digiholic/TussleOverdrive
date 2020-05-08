using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

/// <summary>
/// The SpriteInfo object holds references to all of the ImageDefinitions and Animations a BattleObject needs.
/// </summary>
[System.Serializable]
public class SpriteInfo : IJsonInfoObject
{
    public string spriteSheetFileName;
    public int numRows;
    public int numCols;

    public string defaultAnimation = "idle";
    public List<AnimationDefinition> animations;
    
    [System.NonSerialized] private Dictionary<string, AnimationDefinition> animationsByName = new Dictionary<string, AnimationDefinition>();
    public Texture2D spriteSheetTexture { get; private set; }

    public SpriteInfo(){
        animations = new List<AnimationDefinition>();
    }

    public void LoadDirectory(string directory_name, FighterInfo info)
    {
        string fullSpritePath = FileLoader.PathCombine(FileLoader.GetFighterPath(directory_name), spriteSheetFileName);
        spriteSheetTexture = FileLoader.LoadTexture(fullSpritePath);
    }

    public static SpriteInfo LoadSpritesFromFile(string directory, string filename = "sprite_info.json")
    {
        if (filename == null) filename = "sprite_info.json";

        string dir = FileLoader.GetFighterPath(directory);
        string combinedPath = Path.Combine(dir, filename);
        if (File.Exists(combinedPath))
        {
            string json = File.ReadAllText(combinedPath);
            SpriteInfo info = JsonUtility.FromJson<SpriteInfo>(json);
            return info;
        }
        else
        {
            Debug.LogWarning("No sprites file found at " + directory + "/" + filename);
            return null;
        }
    }
    #region IJsonInfoObject Implementation
    [SerializeField]
    private TextAsset JSONFile;

    public void LoadFromTextAsset()
    {
        if (JSONFile != null)
        {
            JsonUtility.FromJsonOverwrite(JSONFile.text, this);
        }
    }

    public FileInfo Save(string path)
    {
        FileInfo fileSavedTo = new FileInfo(path);
        string json = JsonUtility.ToJson(this, true);
        File.WriteAllText(path, json);
        Debug.Log("Saving Sprite Info to: "+path);
        Debug.Log(json);
        return fileSavedTo;
    }
    #endregion
}
