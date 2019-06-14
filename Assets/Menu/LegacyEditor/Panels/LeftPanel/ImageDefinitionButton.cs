using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ImageDefinitionButton : MonoBehaviour
{
    public UILabel label;
    private UIButton button;
    public ImageDefinition imageDef;

    // Use this for initialization
    void Awake()
    {
        label = GetComponentInChildren<UILabel>();
        button = GetComponent<UIButton>();
        ChangedImageDef(LegacyEditorData.instance.currentImageDef);
    }

    public void SetImageDef(ImageDefinition def)
    {
        imageDef = def;
        label.text = def._imageName;
        ModifyLegacyEditorDataAction legacyAction = ScriptableObject.CreateInstance<ModifyLegacyEditorDataAction>();
        legacyAction.init("currentImageDef", def);
        legacyAction.enableDeselect();

        GetComponent<OnClickSendAction>().action = legacyAction;
    }

    void ChangedImageDef(ImageDefinition def)
    {
        if (def != null && def == imageDef)
        {
            button.defaultColor = new Color(1, 1, 1, 1);
        }
        else
        {
            button.defaultColor = new Color(1, 1, 1, 0.5f);
        }
    }

    private void OnEnable()
    {
        LegacyEditorData.instance.CurrentImageDefinitionChangedEvent += ChangedImageDef;
    }

    private void OnDisable()
    {
        LegacyEditorData.instance.CurrentImageDefinitionChangedEvent -= ChangedImageDef;
    }
}
