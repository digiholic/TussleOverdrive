﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuilderFighter : LegacyEditorWidget{
    public GameObject fighterObject;

    void OnFighterChanged(FighterInfo info)
    {
        if (info.action_file != null && !info.action_file.Equals("") && info.getSpriteData() != null)
        {
            fighterObject.SetActive(true);
            fighterObject.SendMessage("OnFighterInfoReady", info);
            SpriteHandler spriteHandler = fighterObject.GetComponent<SpriteHandler>();
            spriteHandler.ChangeSprite("idle");
            spriteHandler.ChangeSubimage(0);
        } else
        {
            Debug.Log("Fighter does not have enough data to be enabled");
            fighterObject.SetActive(false);
        }
    }

    void OnFrameChange(int frame)
    {
        UpdateFighterAction();
    }

    void OnActionChange(DynamicAction act)
    {
        UpdateFighterAction();
    }

    void UpdateFighterAction()
    {
        ActionHandler actionHandler = fighterObject.GetComponent<ActionHandler>();
        actionHandler.DoAction(editor.currentAction);
        while (actionHandler.CurrentAction.current_frame < editor.currentFrame)
        {
            actionHandler.ManualUpdate();
        }
    }

    public override void RegisterListeners()
    {
        editor.FighterInfoChangedEvent += OnFighterChanged;
        editor.CurrentActionChangedEvent += OnActionChange;
    }

    public override void UnregisterListeners()
    {
        editor.FighterInfoChangedEvent -= OnFighterChanged;
        editor.CurrentActionChangedEvent -= OnActionChange;
    }
}