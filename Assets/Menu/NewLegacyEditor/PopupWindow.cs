using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class PopupWindow : MonoBehaviour {
    public static PopupWindow current_popup_manager;
    private UIPanel PopupPanel;

    public GameObject BlockerPanel;
    public FileBrowser FileBrowserPanel;

    void Awake()
    {
        current_popup_manager = this;
    }

    public static void OpenFileBrowser(DirectoryInfo starting_directory, FileBrowser.ValidateFile validate_function, FileBrowser.FileCallback callback_function)
    {
        current_popup_manager.BlockerPanel.SetActive(true);
        current_popup_manager.FileBrowserPanel.gameObject.SetActive(true);
        current_popup_manager.FileBrowserPanel.Initialize(starting_directory, validate_function, callback_function);
    }

    public void CloseFileBrowser()
    {
        BlockerPanel.SetActive(false);
        FileBrowserPanel.RemoveData();
        FileBrowserPanel.gameObject.SetActive(false);
    }
}
