using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;


public class FileLoader {

    public static DirectoryInfo ProgramDirectoryInfo = new DirectoryInfo(Application.dataPath).Parent;

    public static string FighterPath = PathCombine("Assets", "Modules", "Fighters");
    public static string StagePath = PathCombine("Assets", "Modules", "Stages");

    public static DirectoryInfo FighterDir = new DirectoryInfo(FighterPath);
    public static DirectoryInfo StageDir = new DirectoryInfo(StagePath);

    public static string PathCombine(params string [] paths)
    {
        string ResultString = paths[0];
        
        for (int i=1; i<paths.Length; i++)
        {
            ResultString = Path.Combine(ResultString, paths[i]);
        }
        return ResultString;
    }

    public static string GetFighterPath(string dirname)
    {
        return PathCombine(FighterPath, dirname);
    }

    public static DirectoryInfo GetFighterDir(string dirname)
    {
        return new DirectoryInfo(PathCombine(FighterPath, dirname));
    }

    public static string GetStagePath(string dirname)
    {
        return PathCombine(StagePath, dirname);
    }

    public static Texture2D LoadTexture(string FilePath)
    {
        string FullPath = Path.Combine(ProgramDirectoryInfo.FullName, FilePath);

        Texture2D Tex2D;
        byte[] FileData;

        if (File.Exists(FullPath))
        {
            FileData = File.ReadAllBytes(FullPath);
            Tex2D = new Texture2D(2, 2);           // Create new "empty" texture
            if (Tex2D.LoadImage(FileData))           // Load the imagedata into the texture (size is set automatically)
            {
                Tex2D.filterMode = FilterMode.Point;
                return Tex2D;                 // If data = readable -> return texture
            }

        } else
        {
            Debug.LogError("File Does Not Exist: " + FullPath);
        }
        Debug.LogError("Error Loading Texture2D from File " + FilePath);
        return null;                     // Return null if load failed
    }

    public static Sprite LoadSprite(string FilePath)
    {
        Texture2D texture = LoadTexture(FilePath);
        Sprite newSprite = Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), new Vector2(0.5f,0.5f));
        return newSprite;
    }
    public static string LoadTextFile(string FilePath)
    {
        string FullPath = Path.Combine(ProgramDirectoryInfo.FullName, FilePath);

        string data;

        if (File.Exists(FullPath))
        {
            data = File.ReadAllText(FullPath);
            return data;
        }
        Debug.LogError("Error Loading Text from File " + FullPath);
        return "";
    }

    public static string GetPathFromDir(DirectoryInfo start_directory, FileInfo destination_file)
    {
        string file_string = destination_file.FullName;
        string directory_string = start_directory.FullName;
        if (file_string.StartsWith(directory_string))
        {
            return file_string.Remove(0, directory_string.Length + 1); //We add one here to remove the leading slash
        }
        return null;
    }

    public static string GetPathFromDir(DirectoryInfo start_directory, DirectoryInfo destination_file)
    {
        string file_string = destination_file.FullName;
        string directory_string = start_directory.FullName;
        if (file_string.StartsWith(directory_string))
        {
            return file_string.Remove(0, directory_string.Length + 1); //We add one here to remove the leading slash
        }
        return null;
    }
}