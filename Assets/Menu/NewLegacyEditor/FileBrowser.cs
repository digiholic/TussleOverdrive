using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class FileBrowser : MonoBehaviour {
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

    public delegate void FileCallback(FileInfo finfo);
    public FileCallback callback_function;

    void Start()
    {
        Initialize(FileLoader.FighterDir, ValidateFighter, null);
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
    
    public void ConfirmSelection()
    {
        PopupWindow.current_popup_manager.CloseFileBrowser();
        callback_function(current_file);
    }

    public void RemoveData()
    {
        foreach(FileBrowserDataRow data in data_rows)
        {
            NGUITools.Destroy(data.gameObject);
        }
        data_rows = new List<FileBrowserDataRow>();
    }

    void LoadData()
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

    public static bool ValidateFighter(FileInfo info)
    {
        return (info.Extension == ".json");
    }

    public static bool ValidateImage(FileInfo info)
    {
        return (info.Extension == ".png" || info.Extension == ".jpg");
    }
}
