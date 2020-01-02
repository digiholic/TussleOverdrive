using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Rewired;

public class InputBuffer : MonoBehaviour {
    //How much the joystick needs to move in one frame to be called a "smash"
    private const float SMASH_DELTA_THRESHOLD = 0.65f;
    //The minimum value of a joystick for a "smash" to be counted, after the delta has been achieved
    private const float SMASH_VALUE_THRESHOLD = 0.8f; 

    public int player_num = -1;

    private List<ButtonBuffer> input_buffer = new List<ButtonBuffer>();
    private Player player;

    private BattleObject battleObject;

    void Awake()
    {
        battleObject = GetComponent<BattleObject>();
    }
    
    // Use this for initialization
    void Start () {
        //If the player number isn't set in editor, get it from the battleobject
        if (player_num == -1) player_num = battleObject.GetIntVar(TussleConstants.FighterVariableNames.PLAYER_NUM);
        
        player = ReInput.players.GetPlayer(player_num);

        player.AddInputEventDelegate(ButtonPressed, UpdateLoopType.Update, InputActionEventType.ButtonJustPressed);
        player.AddInputEventDelegate(ButtonReleased, UpdateLoopType.Update, InputActionEventType.ButtonJustReleased);
    }

    void Update()
    {
        if (player.controllers.Joysticks.Count == 0)
        {
            //For the keyboard, smashes are double taps
            //Right Smash
            if (player.GetButtonDoublePressDown("Horizontal")){
                input_buffer.Insert(0, new ButtonBuffer(BattleController.current_battle.current_game_frame, "RightSmash", true));
                //Debug.Log("Right Smash");
            }
            //Left Smash
            if (player.GetNegativeButtonDoublePressDown("Horizontal")) {
                input_buffer.Insert(0, new ButtonBuffer(BattleController.current_battle.current_game_frame, "LeftSmash", true));
                //Debug.Log("Left Smash");
            }
            //Up Smash
            if (player.GetButtonDoublePressDown("Vertical")) {
                input_buffer.Insert(0, new ButtonBuffer(BattleController.current_battle.current_game_frame, "UpSmash", true));
                //Debug.Log("Up Smash");
            }
            //Left Smash
            if (player.GetNegativeButtonDoublePressDown("Vertical")) {
                input_buffer.Insert(0, new ButtonBuffer(BattleController.current_battle.current_game_frame, "DownSmash", true));
                //Debug.Log("Down Smash");
            }

        }
        else
        {
            //If the joystick has moved a lot...
            if (Mathf.Abs(player.GetAxisDelta("Horizontal")) > SMASH_DELTA_THRESHOLD)
            {
                //...And is out far enough to count as a smash
                if (player.GetAxis("Horizontal") > SMASH_VALUE_THRESHOLD) {
                    input_buffer.Insert(0, new ButtonBuffer(BattleController.current_battle.current_game_frame, "RightSmash", true));
                    //Debug.Log("Right Smash");
                }
                if (player.GetAxis("Horizontal") < -SMASH_VALUE_THRESHOLD) {
                    input_buffer.Insert(0, new ButtonBuffer(BattleController.current_battle.current_game_frame, "LeftSmash", true));
                    //Debug.Log("Left Smash");
                }
            }
            if (Mathf.Abs(player.GetAxisDelta("Vertical")) > SMASH_DELTA_THRESHOLD)
            {
                if (player.GetAxis("Vertical") > SMASH_VALUE_THRESHOLD) {
                    input_buffer.Insert(0, new ButtonBuffer(BattleController.current_battle.current_game_frame, "UpSmash", true));
                    //Debug.Log("Up Smash");
            }
                if (player.GetAxis("Vertical") < -SMASH_VALUE_THRESHOLD) {
                    input_buffer.Insert(0, new ButtonBuffer(BattleController.current_battle.current_game_frame, "DownSmash", true));
                    //Debug.Log("Down Smash");
                }
            }
        }
    }

    private void ButtonPressed(InputActionEventData data)
    {
        input_buffer.Insert(0, new ButtonBuffer(BattleController.current_battle.current_game_frame,data.actionName,true));
    }

    private void ButtonReleased(InputActionEventData data)
    {
        input_buffer.Insert(0, new ButtonBuffer(BattleController.current_battle.current_game_frame, data.actionName, false));
    }

    public bool CheckBuffer(string input)
    {
        return CheckBuffer(input, PlayerPrefs.GetInt("buffer_window", 12), true);
    }

    void OnDestroy()
    {
        player.RemoveInputEventDelegate(ButtonPressed);
        player.RemoveInputEventDelegate(ButtonReleased);
    }

    public bool CheckBuffer(string input, int buffer_window, bool pressed)
    {
        foreach (ButtonBuffer bufferedInput in input_buffer)
        {
            //If we've fallen off our distance value, quit the for loop.
            if (bufferedInput.frame < BattleController.current_battle.current_game_frame - buffer_window)
                break;
            else
            {
                //If the key and value match what we're looking for, return true
                if (bufferedInput.action == input && bufferedInput.state == pressed)
                    return true;  
            }
        }
        return false;
    }

    public bool CheckBufferForValid(string input)
    {
        return CheckBufferForValid(input, PlayerPrefs.GetInt("buffer_window", 12), true);
    }

    public bool CheckBufferForValid(string input, int buffer_window, bool pressed)
    {
        foreach (ButtonBuffer bufferedInput in input_buffer)
        {
            //If we've fallen off our distance value, quit the for loop.
            if (bufferedInput.frame < BattleController.current_battle.current_game_frame - buffer_window)
                break;
            else
            {
                //If the key and value match what we're looking for, return true
                if (bufferedInput.active && bufferedInput.action == input && bufferedInput.state == pressed)
                    return true;
            }
        }
        return false;
    }

    public bool KeyBuffered(string input)
    {
        return KeyBuffered(input, PlayerPrefs.GetInt("buffer_window", 12), true);
    }

    public bool KeyBuffered(string input, int buffer_window, bool pressed)
    {
        foreach (ButtonBuffer bufferedInput in input_buffer)
        {
            //If we've fallen off our distance value, quit the for loop.
            if (bufferedInput.frame < BattleController.current_battle.current_game_frame - buffer_window)
                break;
            else
            {
                //If the key and value match what we're looking for, return it if active.
                if (bufferedInput.action == input && bufferedInput.state == pressed)
                    return bufferedInput.Consume();
            }
        }
        //If we didn't find it, return false.
        return false;
    }

    public bool CheckDoubleTap(string input)
    {
        return CheckDoubleTap(input, PlayerPrefs.GetInt("buffer_window", 12));
    }

    public bool CheckDoubleTap(string input, int buffer_window)
    {
        int index = 0;
        bool[] valuesToFind = new bool[] { true, false, true };

        foreach (ButtonBuffer bufferedInput in input_buffer)
        {
            //If we've fallen off our distance value, quit the for loop.
            if (bufferedInput.frame < BattleController.current_battle.current_game_frame - buffer_window)
                break;
            //If we've already found everything, we're done with the loop and can end early
            else if (index >= valuesToFind.Length)
                break;
            else
            {
                //If the key and value match what we're looking for, return it if active.
                if (bufferedInput.action == input && bufferedInput.state == valuesToFind[index])
                    index++;
            }
        }
        //If we've hit three things in the valuesToFind list, return true. Otherwise, false
        return (index >= valuesToFind.Length);
    }

    public bool GetKey(string input)
    {
        return player.GetButton(input);
    }

    public bool GetKeyDown(string input)
    {
        return player.GetButtonDown(input);
    }

    public bool GetKeyUp(string input)
    {
        return player.GetButtonUp(input);
    }

    public bool DirectionHeld(string direction)
    {
        if (direction == "Forward")
        {
            if (battleObject.GetIntVar(TussleConstants.FighterVariableNames.FACING_DIRECTION) == 1) direction = "Right";
            if (battleObject.GetIntVar(TussleConstants.FighterVariableNames.FACING_DIRECTION) == -1) direction = "Left";
        }

        if (direction == "Backward")
        {
            if (battleObject.GetIntVar(TussleConstants.FighterVariableNames.FACING_DIRECTION) == 1) direction = "Left";
            if (battleObject.GetIntVar(TussleConstants.FighterVariableNames.FACING_DIRECTION) == -1) direction = "Right";
        }

        if (direction == "Left") return player.GetAxis("Horizontal") < -0.2f;
        if (direction == "Right") return player.GetAxis("Horizontal") > 0.2f;
        if (direction == "Up") return player.GetAxis("Vertical") > 0.2f;
        if (direction == "Down") return player.GetAxis("Vertical") < -0.2f;
        
        return false;
    }

    public float GetAxis(string axis)
    {
        return player.GetAxis(axis);
    }

    public float GetAxisDelta(string axis){
        return player.GetAxisDelta(axis);
    }
}

[System.Serializable]
public class InputBufferFrame
{
    public int frame;

    public float x_axis;
    public float y_axis;

    //These will be enabled on the frame you first press the button
    public bool AttackPressed;
    public bool SpecialPressed;
    public bool JumpPressed;
    public bool ShieldPressed;
    
    //These will be enabled on the frame you release the button
    public bool AttackReleased;
    public bool SpecialReleased;
    public bool JumpReleased;
    public bool ShieldReleased;
    
}

public class ButtonBuffer
{
    public int frame;
    public string action;
    public bool state;

    public bool active; //If it's active, it can be consumed, which will unset the active

    public ButtonBuffer(int frame, string action, bool state)
    {
        this.frame = frame;
        this.action = action;
        this.state = state;
        active = true;
    }

    public bool Consume()
    {
        if (active)
        {
            active = false;
            return true;
        }
        return false;
    }
}