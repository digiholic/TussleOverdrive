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
    private string fighter_directory; //The fighter's directory
    private string fullSpriteDirectoryName; //The fighter's directory plus the sprite directory

    public string default_sprite = "idle";
    public string spriteDirectory = "sprites";
    public string costumeName;
    public List<AnimationDefinition> animations;
    
    [System.NonSerialized]
    private Dictionary<string, AnimationDefinition> animationsByName = new Dictionary<string, AnimationDefinition>();
    
    /// <summary>
    /// Load a directory of sprites, caching their images for future use
    /// Needs to be passed a fighter path to the directory to load relative from.
    /// </summary>
    /// <param name="directoryName"></param>
    public void LoadDirectory(string fighterDirName)
    {
        fighter_directory = FileLoader.GetFighterPath(fighterDirName);
        fullSpriteDirectoryName = FileLoader.PathCombine(fighter_directory, spriteDirectory);
        
        //Iterate over each animation and cache each subimage
        foreach (AnimationDefinition aData in animations)
        {
            animationsByName.Add(aData.animationName, aData);
            foreach (ImageDefinition sData in aData.subimages)
            {
                sData.cacheSprite(fullSpriteDirectoryName, costumeName);
            }
        }
    }

    public Sprite getSpriteFromAnimation(string name, int frame=-1)
    {
        AnimationDefinition anim = getAnimationByName(name);
        ImageDefinition imageDef = anim.getCurrentSubimage();
        //By default, frame is -1, which is interpreted as "current". If it's above zero, we'll get that image
        //Note that this won't set the current frame
        if (frame >= 0)
        {
            imageDef = anim.getImageForFrame(frame);
        }
        return imageDef.getSprite(fullSpriteDirectoryName, costumeName);
    }

    public AnimationDefinition getAnimationByName(string name)
    {
        if (animationsByName.ContainsKey(name))
        {
            return animationsByName[name];
        }
        else
        {
            Debug.LogError("No animation inside SpriteInfo named " + name);
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
        return fileSavedTo;
    }
    #endregion
}
