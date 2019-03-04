using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class InputFilePickerButton : MonoBehaviour
{
    [SerializeField]
    private UIInput input;
    [SerializeField]
    private FileBrowser browser;
    [SerializeField]
    ValidationType fileType;

    private void OnAction()
    {
        //Open File Browser
        DirectoryInfo fighterDir = FileLoader.GetFighterDir(LegacyEditorData.instance.FighterDirName);
        switch (fileType)
        {
            case ValidationType.JSON:
                browser.BrowseForJSON(fighterDir, LoadFileCallback);
                break;
            case ValidationType.IMAGE:
                browser.BrowseForImage(fighterDir, LoadFileCallback);
                break;
            case ValidationType.ALL:
                browser.BrowseForImage(fighterDir, LoadFileCallback);
                break;
            case ValidationType.DIRECTORY:
                browser.BrowseForDirectory(fighterDir, LoadDirectoryCallback);
                break;
        }
    }
    
    private bool LoadFileCallback(FileInfo fileInput)
    {
        DirectoryInfo fighterDir = FileLoader.GetFighterDir(LegacyEditorData.instance.FighterDirName);
        string relativePath = FileLoader.GetPathFromDir(fighterDir, fileInput);
        if (relativePath == null)
        {
            browser.SetErrorText("Fighter files must be inside the fighter directory");
            return false;
        }
        ModifyInput(relativePath);
        return true;
    }
    
    private bool LoadDirectoryCallback(DirectoryInfo directoryInput)
    {
        DirectoryInfo fighterDir = FileLoader.GetFighterDir(LegacyEditorData.instance.FighterDirName);
        string relativePath = FileLoader.GetPathFromDir(fighterDir, directoryInput);
        if (relativePath == null)
        {
            browser.SetErrorText("Fighter files must be inside the fighter directory");
            return false;
        }
        ModifyInput(relativePath);
        return true;
    }

    private void ModifyInput(string selectedInput)
    {
        //Doing a bit of dark voodoo magic to call this private method in a library without modifying the library
        //This will let us do everything the UIInput does as if we typed it in
        Debug.Log(selectedInput);
        input.value = "";
        input.SendMessage("Append", selectedInput + '\n');
    }

    public enum ValidationType
    {
        JSON,
        IMAGE,
        DIRECTORY,
        ALL
    }
}
