using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class FileBrowserDataRow : MonoBehaviour {
    public static int last_depth = 26;
    public UISprite FolderIcon;
    public UISprite FileIcon;
    public UISprite ImageIcon;
    public UILabel Filename;

    public FileInfo current_file;
    public DirectoryInfo current_directory;

    public FileBrowser browser;

    // Use this for initialization
    public void Initialize()
    {
        if (current_file != null)
        {
            Filename.text = current_file.Name;
            if (current_file.Extension == ".png")
                ImageIcon.gameObject.SetActive(true);
            else
                FileIcon.gameObject.SetActive(true);
        }
        else if (current_directory != null)
        {
            Filename.text = current_directory.Name;
            FolderIcon.gameObject.SetActive(true);
        }
    }

	public void ChangeName(string text)
    {
        Filename.text = text;
    }

    public void OnClick()
    {
        if (current_directory != null)
        {
            Debug.Log(browser);
            browser.ChangeDirectory(current_directory);
        }
        else if (current_file != null)
        {
            if (browser.validate_method(current_file))
            {
                browser.current_file = current_file;
                browser.SelectedFileText.text = current_file.Name;
            }
        }
    }
}
