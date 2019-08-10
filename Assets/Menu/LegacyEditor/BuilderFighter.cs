using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuilderFighter : LegacyEditorWidget{
    public GameObject fighterObject;
    private SpriteRenderer displaySprite;

    void OnFighterChanged(FighterInfo info)
    {
        if (info.action_file != null && info.sprite_info != null)
        {
            fighterObject.SetActive(true);
            fighterObject.SendMessage("OnFighterInfoReady", info);
            SpriteHandler spriteHandler = fighterObject.GetComponent<SpriteHandler>();
            spriteHandler.ChangeAnimation("idle", 0);
            displaySprite = fighterObject.GetComponentInChildren<SpriteRenderer>();
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

    void UpdateImageDef(ImageDefinition def)
    {
        Sprite spr = def?.getSprite(editor.loadedSpriteInfo.fullSpriteDirectoryName, editor.loadedSpriteInfo.costumeName);
        Debug.Log(spr);
        if (spr != null)
        {
            displaySprite.sprite = spr;
        } else
        {
            Debug.Log("No cached sprite for " + def);
        }
    }

    public override void RegisterListeners()
    {
        editor.FighterInfoChangedEvent += OnFighterChanged;
        editor.CurrentActionChangedEvent += OnActionChange;
        editor.CurrentImageDefinitionChangedEvent += UpdateImageDef;
    }

    public override void UnregisterListeners()
    {
        editor.FighterInfoChangedEvent -= OnFighterChanged;
        editor.CurrentActionChangedEvent -= OnActionChange;
        editor.CurrentImageDefinitionChangedEvent -= UpdateImageDef;
    }
}
