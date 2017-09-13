using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class PopupWindow : MonoBehaviour {
    private static PopupWindow current_popup_manager;
    private UIPanel PopupPanel;

    public GameObject BlockerPanel;
    public FileBrowser FileBrowserPanel;

    void Awake()
    {
        current_popup_manager = this;
    }

    public static void OpenFileBrowser(DirectoryInfo starting_directory, FileBrowser.ValidateFile validate_function, FileBrowser.FileCallback callback_function)
    {
        current_popup_manager.FileBrowserPanel.Initialize(starting_directory, validate_function, callback_function);
    }
}
