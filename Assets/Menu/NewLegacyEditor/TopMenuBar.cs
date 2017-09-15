using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class TopMenuBar : MonoBehaviour {
    
    public void OpenFighterBrowser()
    {
        Debug.Log("Opening Fighter Browser");
        PopupWindow.current_popup_manager.OpenFileBrowser(FileLoader.FighterDir, FileBrowser.ValidateFighter, FighterPopupCallback);
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
            PopupWindow.current_popup_manager.OpenInfoBox("Could not find a fighter at " + file_info.Name + " Maybe the file is malformed, or an incorrect json file?");
        }
    }
}
