using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class FileBrowser : MonoBehaviour {
    delegate void FileBrowserCallback(FileInfo chosenFile);

    public PopupWindow main_popup;
    public UIGrid gridPanel;
    public FileBrowserDataRow data_row_prefab;
    public FileBrowserDataRow up_one_level;
    public UILabel SelectedFileText;

    public DirectoryInfo current_directory;
    public FileInfo current_file;

    private List<FileBrowserDataRow> data_rows = new List<FileBrowserDataRow>();
    
    public delegate bool ValidateFile(FileInfo finfo);
    public ValidateFile validate_method;

    public delegate bool FileCallback(FileInfo finfo);
    public FileCallback callback_function;

    bool LoadFighter(FileInfo info)
    {
        FighterInfo fInfo = FighterInfo.LoadFighterInfoFile(info.DirectoryName,info.Name);
        if (fInfo != null)
        {
            
            LegacyEditorData.instance.LoadNewFighter(fInfo);
            return true;
        }
        return false;
    }

    void Start()
    {
        Initialize(FileLoader.FighterDir, ValidateFighter, LoadFighter);
    }

    public void Initialize(DirectoryInfo starting_directory, ValidateFile validation_method, FileCallback callback)
    {
        validate_method = validation_method;
        current_directory = starting_directory;
        callback_function = callback;
        LoadData();
    }

    public void ChangeDirectory(DirectoryInfo new_directory)
    {
        GetComponentInChildren<UIDraggablePanel>().ResetPosition();
        RemoveData();
        current_directory = new_directory;
        LoadData();
    }

    public void RemoveData()
    {
        foreach (FileBrowserDataRow data in data_rows)
        {
            NGUITools.Destroy(data.gameObject);
        }
        data_rows = new List<FileBrowserDataRow>();
        BroadcastMessage("RefuckLabelDepth", SendMessageOptions.DontRequireReceiver);
    }

    void ConfirmSelection()
    {
        //PopupWindow.current_popup_manager.CloseFileBrowser();
        if (callback_function != null && current_file != null && current_file.Exists)
        {
            bool callbackSuccessful = callback_function(current_file);
            if (callbackSuccessful)
            {
                Dispose();
            }
        }
    }

    public void Dispose()
    {
        NGUITools.SetActive(transform.parent.gameObject, false);
    }

    private void LoadData()
    {
        up_one_level.current_directory = current_directory.Parent;
        foreach(DirectoryInfo directory in current_directory.GetDirectories())
        {
            InstantiateDirectoryRow(directory);
        }
        foreach(FileInfo fname in current_directory.GetFiles())
        {
            if (validate_method(fname))
                InstantiateFileRow(fname);
        }
        gridPanel.Reposition();
        BroadcastMessage("UnfuckLabelDepth",SendMessageOptions.DontRequireReceiver);
    }

    private FileBrowserDataRow InstantiateDirectoryRow(DirectoryInfo directory)
    {
        GameObject go = NGUITools.AddChild(gridPanel.gameObject, data_row_prefab.gameObject);
        FileBrowserDataRow data = go.GetComponent<FileBrowserDataRow>();
        data.current_directory = directory;
        data.browser = this;
        data.Initialize();
        data_rows.Add(data);
        return data;
    }

    private FileBrowserDataRow InstantiateFileRow(FileInfo fname)
    {
        GameObject go = NGUITools.AddChild(gridPanel.gameObject, data_row_prefab.gameObject);
        FileBrowserDataRow data = go.GetComponent<FileBrowserDataRow>();
        data.current_file = fname;
        data.browser = this;
        data.Initialize();
        data_rows.Add(data);
        return data;
    }

    #region static validators
    public static bool ValidateFighter(FileInfo info)
    {
        return (info.Extension == ".json");
    }

    public static bool ValidateImage(FileInfo info)
    {
        return (info.Extension == ".png" || info.Extension == ".jpg");
    }

    public static bool ValidateEverything(FileInfo info)
    {
        return true;
    }

    public static bool LoadFighterCallback(FileInfo file_info)
    {
        FighterInfo fighter_info = JsonUtility.FromJson<FighterInfo>(File.ReadAllText(file_info.FullName));
        if (fighter_info.display_name != null)
        {
            LegacyEditor.editor.fighter_file = file_info;
            fighter_info.LoadDirectory(file_info.DirectoryName);
            LegacyEditor.FireChangeFighter(fighter_info);
            return true;
        }
        else
        {
            PopupWindow.current_popup_manager.OpenInfoBox("Could not find a fighter at " + file_info.Name + " Maybe the file is malformed, or an incorrect json file?");
            return false;
        }
    }
    #endregion
}
