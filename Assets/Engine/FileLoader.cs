using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;


public class FileLoader {

    public static DirectoryInfo ProgramDirectoryInfo = new DirectoryInfo(Application.dataPath).Parent;

    public static string PathCombine(params string [] paths)
    {
        string ResultString = paths[0];
        
        for (int i=1; i<paths.Length; i++)
        {
            ResultString = Path.Combine(ResultString, paths[i]);
        }
        return ResultString;
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

        }
        Debug.LogError("Error Loading Texture2D from File " + FilePath);
        return null;                     // Return null if load failed
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
}
