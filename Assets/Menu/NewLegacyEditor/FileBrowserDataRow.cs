using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class FileBrowserDataRow : MonoBehaviour {
    public GameObject FolderIcon;
    public GameObject FileIcon;
    public GameObject ImageIcon;
    public UILabel Filename;

    public FileInfo current_file;
    public DirectoryInfo current_directory;

	// Use this for initialization
	public void Initialize() {
        if (current_file != null)
        {
            Filename.text = current_file.Name;
            if (current_file.Extension == ".png")
                ImageIcon.SetActive(true);
            else
                FileIcon.SetActive(true);
        }
        else if (current_directory != null)
        {
            Filename.text = current_directory.Name;
            FolderIcon.SetActive(true);
        }
    }
	
	public void ChangeName(string text)
    {
        Filename.text = text;
    }
}
