using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class FileBrowser : MonoBehaviour {
    public FileBrowserDataRow data_row_prefab;

    public DirectoryInfo current_directory;
    private List<FileBrowserDataRow> data_rows;

    void Start()
    {
        current_directory = FileLoader.FighterDir;
        LoadData();
    }

    
    void RemoveData()
    {
        foreach(FileBrowserDataRow data in data_rows)
        {
            Destroy(data.gameObject);
        }
    }

    void LoadData()
    {
        FileBrowserDataRow data = InstantiateDirectoryRow(current_directory.Parent);
        data.ChangeName("../");
        foreach(DirectoryInfo directory in current_directory.GetDirectories())
        {
            InstantiateDirectoryRow(directory);
        }
    }

    private FileBrowserDataRow InstantiateDirectoryRow(DirectoryInfo directory)
    {
        GameObject go = NGUITools.AddChild(gameObject, data_row_prefab.gameObject);
        FileBrowserDataRow data = go.GetComponent<FileBrowserDataRow>();
        data.current_directory = directory;
        data.Initialize();
        return data;
    }
}
