using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class ButtonLoadFile : MonoBehaviour {
    public string variable_name;
    public FileBrowserType file_type;
    
    void OnPressed()
    {
        if (file_type == FileBrowserType.Image)
            PopupWindow.current_popup_manager.OpenFileBrowser(LegacyEditor.CurrentFighterDir(), FileBrowser.ValidateImage, CallbackFunction);
        else if (file_type == FileBrowserType.Json)
            PopupWindow.current_popup_manager.OpenFileBrowser(LegacyEditor.CurrentFighterDir(), FileBrowser.ValidateFighter, CallbackFunction);
    }

    bool CallbackFunction(FileInfo info)
    {
        string value = FileLoader.GetPathFromDir(LegacyEditor.CurrentFighterDir(), info);
        FighterInfo fighter = LegacyEditor.editor.current_fighter;
        fighter.GetType().GetField(variable_name).SetValue(fighter, value);
        LegacyEditor.FireChangeFighter(fighter);
        return true;
    }

    public enum FileBrowserType
    {
        Image, Json
    }
}
