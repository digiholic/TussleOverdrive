﻿using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

/// <summary>
/// The SpriteInfo object holds references to all of the ImageDefinitions and Animations a BattleObject needs.
/// </summary>
[System.Serializable]
public class SpriteInfo : IJsonInfoObject
{
    [System.NonSerialized]
    public string fighter_directory; //The fighter's directory
    [System.NonSerialized]
    public string fullSpriteDirectoryName; //The fighter's directory plus the sprite directory

    public string default_sprite = "idle";
    public string spriteDirectory = "sprites";
    public string costumeName;

    public List<AnimationDefinition> animations;
    public List<ImageDefinition> imageDefinitions;

    public List<FileInfo> spriteFiles { get; private set; }
    

    [System.NonSerialized]
    private Dictionary<string, AnimationDefinition> animationsByName = new Dictionary<string, AnimationDefinition>();
    

    public SpriteInfo(){
        animations = new List<AnimationDefinition>();
        imageDefinitions = new List<ImageDefinition>();
        spriteFiles = new List<FileInfo>();
    }

    /// <summary>
    /// Load a directory of sprites, caching their images for future use
    /// Needs to be passed a fighter path to the directory to load relative from.
    /// </summary>
    /// <param name="directoryName"></param>
    public void LoadDirectory(string fighterDirName,FighterInfo fighter_info)
    {
        if (animations == null) animations = new List<AnimationDefinition>();
        if (imageDefinitions == null) imageDefinitions = new List<ImageDefinition>();

        fighter_directory = FileLoader.GetFighterPath(fighterDirName);
        fullSpriteDirectoryName = FileLoader.PathCombine(fighter_directory, spriteDirectory);
        
        //Iterate over each animation and cache each subimage
        foreach (AnimationDefinition aData in animations)
        {
            animationsByName.Add(aData.AnimationName, aData);
            foreach (string imageName in aData.subimages)
            {
                ImageDefinition sData = GetImageByName(imageName);
                float ppu = float.Parse(fighter_info.GetVarByName(TussleConstants.SpriteVariableNames.PIXELS_PER_UNIT).value);
                sData.cacheSprite(fullSpriteDirectoryName, costumeName, ppu);
            }
        }

        spriteFiles = new List<FileInfo>();
        //Go through the directory and load the image files
        DirectoryInfo spriteDir = new DirectoryInfo(Path.Combine(fullSpriteDirectoryName,costumeName));
        foreach (FileInfo file in spriteDir.EnumerateFiles())
        {
            //For transparency sake, we only bother with PNG files
            if (file.Extension.EndsWith("png"))
            {
                spriteFiles.Add(file);
            }
        }
    }

    public Sprite getSpriteFromAnimation(string name, int frame=-1)
    {
        AnimationDefinition anim = getAnimationByName(name);
        ImageDefinition imageDef = GetImageByName(anim.getCurrentSubimage());
        //By default, frame is -1, which is interpreted as "current". If it's above zero, we'll get that image
        //Note that this won't set the current frame
        if (frame >= 0)
        {
            imageDef = GetImageByName(anim.getImageForFrame(frame));
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
            Debug.LogWarning("No animation inside SpriteInfo named " + name);
            return AnimationDefinition.NullAnimation;
        }
    }

    public void AddAnimation(AnimationDefinition newDef, bool overwrite = false)
    {
        //Get all of the animations whose names match this one
        List<AnimationDefinition> existingAnimations = animations.FindAll(s => s.AnimationName == newDef.AnimationName);
        if (existingAnimations.Count > 1)
        {
            throw new System.Exception("Multiple Animations with the same name! I told you this would happen!");
        }
        //If there is already an animation, we need to figure out what to do with the old one based on the overwrite flag
        if (existingAnimations.Count > 0)
        {
            if (overwrite)
            {
                animations.RemoveAll(s => s.AnimationName == newDef.AnimationName);
                animations.Add(newDef);
            } else
            {
                AnimationDefinition cloneAnim = new AnimationDefinition(newDef);
                cloneAnim.AnimationName += "_new";
                animations.Add(cloneAnim);
            }
        }
        //If there aren't any existing animations, it's fine to just add it in
        else
        {
            animations.Add(newDef);
        }
    }

    public void DeleteAnimation(AnimationDefinition def)
    {
        animations.Remove(def);
    }

    public ImageDefinition GetImageByName(string name)
    {
        foreach(ImageDefinition def in imageDefinitions)
        {
            if (def.ImageName == name) return def;
        }

        //If we try to load the default sprite and fail, that's a problem.
        if (name == default_sprite){
            Debug.LogError("Could not find image definition for default sprite: " + name + " in SpriteInfo");
            return null;
        }
        
        //If we fail to load a non-default sprite, fall back and try to load that one instead.
        return GetImageByName(default_sprite);
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
