using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimelineFrameCount : LegacyEditorWidget {
    public UILabel text;

    void OnActionChanged(DynamicAction action)
    {
        UpdateCount();
    }

    void OnFrameChanged(int frame)
    {
        UpdateCount();
    }

    void UpdateCount()
    {
        //Since the action can't be null, it turns into an anymous action whenever there's nothing loaded. If it's name is empty, it's basically null
        if (LegacyEditorData.instance.currentAction.name == "")
        {
            text.text = "---";
        }
        else
        {
            text.text = LegacyEditorData.instance.currentFrame.ToString() + "/" + LegacyEditorData.instance.currentAction.length.ToString();
        }
    }

    public override void RegisterListeners()
    {
        editor.CurrentActionChangedEvent += OnActionChanged;
        editor.CurrentFrameChangedEvent += OnFrameChanged;
    }

    public override void UnregisterListeners()
    {
        editor.CurrentActionChangedEvent -= OnActionChanged;
        editor.CurrentFrameChangedEvent -= OnFrameChanged;
    }

    private void Awake()
    {
        SetEditor(LegacyEditorData.instance);
        RegisterListeners();
    }

    private void OnDisable()
    {
        UnregisterListeners();
    }
}
