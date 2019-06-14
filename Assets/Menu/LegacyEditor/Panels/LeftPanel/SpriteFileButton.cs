using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class SpriteFileButton : MonoBehaviour
{
    public UILabel label;
    private UIButton button;
    public FileInfo spriteFile;

    // Use this for initialization
    void Awake()
    {
        label = GetComponentInChildren<UILabel>();
        button = GetComponent<UIButton>();
        ChangedImageFile(LegacyEditorData.instance.currentImageFile);
    }

    void ChangedImageFile(FileInfo file)
    {
        if (file != null && file == spriteFile)
        {
            button.defaultColor = new Color(1, 1, 1, 1);
        }
        else
        {
            button.defaultColor = new Color(1, 1, 1, 0.5f);
        }
    }

    public void SetImageFile(FileInfo file)
    {
        spriteFile = file;
        label.text = file.Name;
        ModifyLegacyEditorDataAction legacyAction = ScriptableObject.CreateInstance<ModifyLegacyEditorDataAction>();
        legacyAction.init("currentImageFile", file);
        legacyAction.enableDeselect();

        GetComponent<OnClickSendAction>().action = legacyAction;
    }

    private void OnEnable()
    {
        LegacyEditorData.instance.CurrentImageFileChangedEvent += ChangedImageFile;
    }

    private void OnDisable()
    {
        LegacyEditorData.instance.CurrentImageFileChangedEvent -= ChangedImageFile;
    }
}
