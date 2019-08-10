using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class FileBrowser : MonoBehaviour {
    delegate void FileBrowserCallback(FileInfo chosenFile);

    [SerializeField]
    private PopupWindow main_popup;
    [SerializeField]
    private UIGrid gridPanel;
    [SerializeField]
    private FileBrowserDataRow data_row_prefab;
    [SerializeField]
    private FileBrowserDataRow up_one_level;
    [SerializeField]
    private PopupErrorLabel errorLabel;
    [SerializeField]
    private UILabel directory_label;
    [SerializeField]
    private UIScrollView dragPanel;

    public UILabel SelectedFileText;

    public DirectoryInfo current_directory;
    public FileInfo current_file;

    private List<FileBrowserDataRow> data_rows = new List<FileBrowserDataRow>();
    
    public delegate bool ValidateFile(FileInfo finfo);
    public ValidateFile validate_method;

    public delegate bool FileCallback(FileInfo finfo);
    public FileCallback callback_function;

    public delegate bool DirectoryCallback(DirectoryInfo dInfo);
    public DirectoryCallback directory_callback;

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
        Initialize(FileLoader.FighterDir, ValidateJSONFile, LoadFighter, null);
    }

    public void SetErrorText(string errorText)
    {
        Debug.Log("Setting Error Text: "+errorText);
        errorLabel.DisplayError(errorText);
    }

    public void Initialize(DirectoryInfo starting_directory, ValidateFile validation_method, FileCallback fileCallback, DirectoryCallback directoryCallback)
    {
        NGUITools.SetActive(gameObject, true);
        SelectedFileText.text = "";
        dragPanel.ResetPosition();
        validate_method = validation_method;
        current_directory = starting_directory;
        callback_function = fileCallback;
        directory_callback = directoryCallback;
        directory_label.text = starting_directory.Name;
        LoadData();
    }

    public void BrowseForJSON(DirectoryInfo starting_directory, FileCallback fileCallback)
    {
        Initialize(starting_directory, ValidateJSONFile, fileCallback, null);
        SetErrorText("Invalid JSON File");
    }

    public void BrowseForImage(DirectoryInfo starting_directory, FileCallback fileCallback)
    {
        Initialize(starting_directory, ValidateImage, fileCallback, null);
        SetErrorText("Invalid Image File");
    }

    public void BrowseForFile(DirectoryInfo starting_directory, FileCallback fileCallback)
    {
        Initialize(starting_directory, ValidateEverything, fileCallback, null);
        SetErrorText("Could Not Open File");
    }

    public void BrowseForDirectory(DirectoryInfo starting_directory, DirectoryCallback directoryCallback)
    {
        Initialize(starting_directory, ValidateNothing, null, directoryCallback);
        SetErrorText("Could Not Open Directory");
    }

    public void ChangeDirectory(DirectoryInfo new_directory)
    {
        GetComponentInChildren<UIScrollView>().ResetPosition();
        RemoveData();
        current_directory = new_directory;
        directory_label.text = new_directory.Name;
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

    public void ConfirmSelection()
    {
        if (directory_callback != null && current_directory != null && current_directory.Exists)
        {
            bool callbackSuccessful = directory_callback(current_directory);
            if (callbackSuccessful)
            {
                Dispose();
            }
            else
            {
                errorLabel.DisplayError();
            }
        }

        if (callback_function != null && current_file != null && current_file.Exists)
        {
            bool callbackSuccessful = callback_function(current_file);
            if (callbackSuccessful)
            {
                Dispose();
            } else
            {
                errorLabel.DisplayError();
            }
        }
    }

    public void Dispose()
    {
        RemoveData();
        NGUITools.SetActive(gameObject, false);
    }

    private void LoadData()
    {
        RemoveData();
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
        //BroadcastMessage("UnfuckLabelDepth",SendMessageOptions.DontRequireReceiver);
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
    public static bool ValidateJSONFile(FileInfo info)
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

    public static bool ValidateNothing(FileInfo info)
    {
        return false;
    }

    public static bool LoadFighterCallback(FileInfo file_info)
    {
        FighterInfo fighter_info = JsonUtility.FromJson<FighterInfo>(File.ReadAllText(file_info.FullName));
        if (fighter_info.display_name != null)
        {
            fighter_info.LoadDirectory(file_info.DirectoryName);
            LegacyEditorData.instance.LoadNewFighter(fighter_info);
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
