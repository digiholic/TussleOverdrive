using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class StageSelectionPanel : MonoBehaviour {
    public StagePortraitRig portraits;

    private SpriteRenderer portraitSprite;
    
    public Sprite random_sprite;

    private StageInfo selected_stage;
    private InputManager inputManager;
    private StageSelectorPanel current_panel;

    // Use this for initialization
    void Start()
    {
        portraitSprite = transform.GetChild(0).GetComponent<SpriteRenderer>();
        inputManager = GetComponent<InputManager>();
        current_panel = portraits.GetPanel(new Vector2(0, 0));
    }

    // Update is called once per frame
    void Update()
    {
        current_panel.selected = true;
        if (current_panel.selectedSprite == "Random")//Special case for random
        {
            portraitSprite.sprite = random_sprite;
        }
        else
        {
            portraitSprite.sprite = current_panel.stage_info.stage_portrait;
        }

        //Route button presses to their proper functions
        if (inputManager.GetKeyDown(InputType.Attack))
            ConfirmPressed();
        if (inputManager.GetKeyDown(InputType.Special))
            CancelPressed();
        if (inputManager.GetKeyDown(InputType.Jump))
            SwitchPressed();
        if (inputManager.GetKeyDown(InputType.Shield))
            RulesPressed();
        if (inputManager.GetKeyDown(InputType.Left))
            DirectionPressed(new Vector2(-1, 0));
        if (inputManager.GetKeyDown(InputType.Right))
            DirectionPressed(new Vector2(1, 0));
        if (inputManager.GetKeyDown(InputType.Up))
            DirectionPressed(new Vector2(0, -1));
        if (inputManager.GetKeyDown(InputType.Down))
            DirectionPressed(new Vector2(0, 1));
    }

    private void ConfirmPressed()
    {
        if (current_panel.selectedSprite == "Random")
            selected_stage = portraits.GetRandomStage();
        else
            selected_stage = current_panel.stage_info;
        SceneManager.LoadScene("stage_" + selected_stage.stage_name);
    }

    private void CancelPressed()
    {
        
    }

    private void SwitchPressed()
    {
        current_panel.random_enabled = !current_panel.random_enabled;
    }

    private void RulesPressed()
    {

    }

    private void DirectionPressed(Vector2 dir)
    {
        Vector2 currentPos = current_panel.GridLoc;
        StageSelectorPanel panel = portraits.GetPanel(currentPos + dir);
        if (panel != null && panel.active)
        {
            current_panel.selected = false;
            current_panel = panel;
        }
    }
}
