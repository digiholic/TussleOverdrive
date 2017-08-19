using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelectionPanel : MonoBehaviour {
    public bool active = false;

    public int playerNum = 0;
    public bool confirmed = false;
    
    public PortraitRig portraits;

    private SpriteRenderer bgSprite;
    private SpriteRenderer portraitSprite;
    public Sprite offline_sprite;
    public Sprite random_sprite;

    private FighterInfo selected_fighter;
    private InputManager inputManager;
    private SelectorPanel current_panel;

	// Use this for initialization
	void Start () {
        bgSprite = GetComponent<SpriteRenderer>();
        bgSprite.color = Settings.current_settings.player_colors[playerNum];
        portraitSprite = transform.GetChild(0).GetComponent<SpriteRenderer>();
        inputManager = GetComponent<InputManager>();
        current_panel = portraits.GetPanel(new Vector2(0, 0));
    }
	
	// Update is called once per frame
	void Update () {
        if (active)
        {
            current_panel.selected[playerNum] = true;
            if (current_panel.selectedSprite == "Random")//Special case for random
            {
                portraitSprite.sprite = random_sprite;
            }
            else
            {
                portraitSprite.sprite = current_panel.fighter_info.css_portrait_sprite;
            }
        }
        else
        {
            current_panel.selected[playerNum] = false;
            portraitSprite.sprite = offline_sprite;
        }

        if (confirmed)
        {
            portraitSprite.sortingOrder = 1;
            current_panel.selected[playerNum] = false;
        }
        else portraitSprite.sortingOrder = -1;


        //Route button presses to their proper functions
        if (inputManager.GetKeyDown("Attack"))
            ConfirmPressed();
        if (inputManager.GetKeyDown("Special"))
            CancelPressed();
        if (inputManager.GetKeyDown("Jump"))
            SwitchPressed();
        if (inputManager.GetKeyDown("Shield"))
            RulesPressed();
        if (inputManager.GetKeyDown("Left"))
            DirectionPressed(new Vector2(-1,0));
        if (inputManager.GetKeyDown("Right"))
            DirectionPressed(new Vector2(1, 0));
        if (inputManager.GetKeyDown("Up"))
            DirectionPressed(new Vector2(0, -1));
        if (inputManager.GetKeyDown("Down"))
            DirectionPressed(new Vector2(0, 1));
    }

    private void ConfirmPressed()
    {
        //If the selection is not active, activate it
        if (!active)
        {
            active = true;
        }
        //If the selection is active, confirm the selection
        else if (!confirmed)
        {
            confirmed = true;
            if (current_panel.selectedSprite == "Random")
                selected_fighter = portraits.GetRandomFighter();
            else
                selected_fighter = current_panel.fighter_info;

            BattleLoader.current_loader.fighters[playerNum] = selected_fighter;
        }
        //If the selection is active, and confirmed
        else
        {
            //Check if anyone else is not confirmed yet
            SelectionRig.selection_rig.CheckStart();
        }
    }

    private void CancelPressed()
    {
        //If a selection is confirmed, deselct it
        if (confirmed)
        {
            confirmed = false;
            BattleLoader.current_loader.fighters[playerNum] = null;
        }
        //If the selection is not confirmed, deactivate the selection
        else if (active)
        {
            active = false;
        }
    }

    private void SwitchPressed()
    {

    }

    private void RulesPressed()
    {

    }

    private void DirectionPressed(Vector2 dir)
    {
        if (active && !confirmed)
        {
            Vector2 currentPos = current_panel.GridLoc;
            SelectorPanel panel = portraits.GetPanel(currentPos + dir);
            if (panel.active)
            {
                current_panel.selected[playerNum] = false;
                current_panel = panel;
            }
        }
    }
}


