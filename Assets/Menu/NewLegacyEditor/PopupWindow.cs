using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class PopupWindow : MonoBehaviour {
    public static PopupWindow current_popup_manager;
    private UIPanel PopupPanel;

    public GameObject BlockerPanel;
    public FileBrowser FileBrowserPanel;
    public InfoBox InfoBoxPanel;

    void Awake()
    {
        current_popup_manager = this;
    }

    public void OpenFileBrowser(DirectoryInfo starting_directory, FileBrowser.ValidateFile validate_function, FileBrowser.FileCallback callback_function)
    {
        BlockerPanel.SetActive(true);
        FileBrowserPanel.gameObject.SetActive(true);
        FileBrowserPanel.Initialize(starting_directory, validate_function, callback_function);
    }

    public void CloseFileBrowser()
    {
        BlockerPanel.SetActive(false);
        FileBrowserPanel.RemoveData();
        FileBrowserPanel.gameObject.SetActive(false);
    }

    public void OpenInfoBox(string text)
    {
        BlockerPanel.SetActive(true);
        InfoBoxPanel.gameObject.SetActive(true);
        InfoBoxPanel.SetText(text);
    }

    public void CloseInfoBox()
    {
        BlockerPanel.SetActive(false);
        InfoBoxPanel.SetText("");
        InfoBoxPanel.gameObject.SetActive(false);
    }
}
