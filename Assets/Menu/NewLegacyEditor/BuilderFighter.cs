using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuilderFighter : MonoBehaviour {
    public GameObject fighterObject;

    void OnModelChanged()
    {
        if (LegacyEditorData.instance.loadedFighterDirty)
        {
            fighterObject.SendMessage("OnFighterInfoReady", LegacyEditorData.instance.loadedFighter);
            SpriteHandler spriteHandler = fighterObject.GetComponent<SpriteHandler>();
            spriteHandler.ChangeSprite("idle");
            spriteHandler.ChangeSubimage(0);
        }
        if (LegacyEditorData.instance.currentFrameDirty || LegacyEditorData.instance.currentActionDirty)
        {
            ActionHandler actionHandler = fighterObject.GetComponent<ActionHandler>();
            actionHandler.DoAction(LegacyEditorData.instance.currentAction);
            while (actionHandler.CurrentAction.current_frame < LegacyEditorData.instance.currentFrame)
            {
                actionHandler.ManualUpdate();
            }
        }
    }
}
