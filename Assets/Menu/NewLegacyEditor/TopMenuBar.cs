using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class TopMenuBar : MonoBehaviour {
    
    public void OpenFighterBrowser()
    {
        Debug.Log("Opening Fighter Browser");
        PopupWindow.OpenFileBrowser(FileLoader.FighterDir, FileBrowser.ValidateFighter, FighterPopupCallback);
    }

    void FighterPopupCallback(FileInfo file_info)
    {
        FighterInfo fighter_info = JsonUtility.FromJson<FighterInfo>(File.ReadAllText(file_info.FullName));
        if (fighter_info.display_name != null)
        {
            Debug.Log("Loaded a fighter: " + fighter_info.display_name);
        }
        else
        {
            Debug.LogWarning("Could not find a fighter at " + file_info.FullName + " Maybe the file is malformed, or an incorrect json file?");
        }
    }
}
