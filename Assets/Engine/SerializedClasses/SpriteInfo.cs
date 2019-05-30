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
    #region old data
    public string sprite_directory;
    public string sprite_prefix;
    public string sprite_default;
    public float sprite_pixelsPerUnit;
    #endregion

    [SerializeField]
    private TextAsset JSONFile;

    private string directory_name;
    private string spriteDirectory;
    private string costumeName;
    private List<AnimationDefinition> animations;
    private List<ImageDefinition> sprites;

    #region IJsonInfoObject Implementation
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
        return fileSavedTo;
    }
    #endregion

    /// <summary>
    /// Load a directory of sprites, caching their images for future use
    /// Needs to be passed a ready-to-use path to the directory to load.
    /// </summary>
    /// <param name="directoryName"></param>
    public void LoadDirectory(string directoryName)
    {
        directory_name = FileLoader.GetFighterPath(directoryName);
        foreach (ImageDefinition sData in sprites)
        {
            string filename = costumeName + "_" + sData.ImageName + ".png";
            string path = FileLoader.PathCombine(directoryName, filename);

            if (sData.getCachedSprite() == null)
            {
                Texture2D cachedTextureFile = FileLoader.LoadTexture(path);
                Sprite newSprite = Sprite.Create(cachedTextureFile, new Rect(sData.OffsetX, sData.OffsetY, sData.Width, sData.Height), sData.Pivot.getAsRelative(sData), sData.PixelsPerUnit);
                sData.cacheSprite(newSprite);
            }
        }
    }
}
