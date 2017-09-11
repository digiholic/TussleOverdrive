using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class FileBrowser : MonoBehaviour {
    public PopupWindow main_popup;
    public UIGrid gridPanel;
    public FileBrowserDataRow data_row_prefab;
    public FileBrowserDataRow up_one_level;


    public DirectoryInfo current_directory;
    private List<FileBrowserDataRow> data_rows = new List<FileBrowserDataRow>();
    
    public delegate bool ValidateFile(FileInfo finfo);
    public ValidateFile validate_method;

    void Start()
    {
        validate_method = ValidateFighter;
        current_directory = FileLoader.FighterDir;
        LoadData();
    }

    public void ChangeDirectory(DirectoryInfo new_directory)
    {
        GetComponentInChildren<UIDraggablePanel>().ResetPosition();
        RemoveData();
        current_directory = new_directory;
        LoadData();
    }
    
    void RemoveData()
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
            if (fname.Extension != ".meta")//Unit meta files, man. Gotta filter that shit out
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

    bool ValidateFighter(FileInfo info)
    {
        if (info.Extension == ".json")
        {
            return true;
        }
        return false;
    }


}
