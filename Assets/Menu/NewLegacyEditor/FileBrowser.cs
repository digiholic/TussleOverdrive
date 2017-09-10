using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class FileBrowser : MonoBehaviour {
    public FileBrowserDataRow data_row_prefab;

    public FileBrowserDataRow up_one_level;

    public DirectoryInfo current_directory;
    private List<FileBrowserDataRow> data_rows = new List<FileBrowserDataRow>();
    private UIGrid grid_manager;

    void Start()
    {
        grid_manager = GetComponent<UIGrid>();
        current_directory = FileLoader.FighterDir;
        LoadData();
    }

    public void ChangeDirectory(DirectoryInfo new_directory)
    {
        GetComponentInParent<UIDraggablePanel>().ResetPosition();
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
        grid_manager.Reposition();
    }

    private FileBrowserDataRow InstantiateDirectoryRow(DirectoryInfo directory)
    {
        GameObject go = NGUITools.AddChild(gameObject, data_row_prefab.gameObject);
        FileBrowserDataRow data = go.GetComponent<FileBrowserDataRow>();
        data.current_directory = directory;
        data.browser = this;
        data.Initialize();
        data_rows.Add(data);
        return data;
    }
}
