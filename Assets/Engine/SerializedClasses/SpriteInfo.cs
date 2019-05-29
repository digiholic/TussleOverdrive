using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

[System.Serializable]
public class SpriteInfo : IJsonInfoObject
{
    public string sprite_directory;
    public string sprite_prefix;
    public string sprite_default;
    public float sprite_pixelsPerUnit;

    private string directory_name;
    private List<ImageDefinition> sprites;

    [SerializeField]
    private TextAsset JSONFile;
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
        return WriteJSON(path);
    }
    #endregion

    private FileInfo WriteJSON(string path)
    {
        FileInfo fileSavedTo = new FileInfo(path);
        string json = JsonUtility.ToJson(this, true);
        File.WriteAllText(path, json);
        return fileSavedTo;
    }

    public void LoadDirectory(string directoryName)
    {
        directory_name = FileLoader.GetFighterPath(directoryName);
        foreach (ImageDefinition sData in sprites)
        {
            //sData.SpriteFileName;
        }
    }
}
