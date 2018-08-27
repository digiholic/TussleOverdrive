using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SaveButton : MonoBehaviour {
    void OnAction()
    {
        FighterInfo info = LegacyEditorData.instance.loadedFighter;
        ActionFile actionFile = LegacyEditorData.instance.loadedActionFile;

        info.Save();
        string path = FileLoader.PathCombine(FileLoader.GetFighterPath(LegacyEditorData.instance.FighterDirName), info.action_file_path);
        Debug.Log(path);
        actionFile.WriteJSON(path);

    }
}
