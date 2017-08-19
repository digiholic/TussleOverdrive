using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Rewired;

public class InputBuffer : BattleComponent {

    private List<ButtonBuffer> input_buffer = new List<ButtonBuffer>();
    private Player player;

    // Use this for initialization
    void Start () {
        player = ReInput.players.GetPlayer(battleObject.GetAbstractFighter().player_num);

        player.AddInputEventDelegate(ButtonPressed, UpdateLoopType.Update, InputActionEventType.ButtonJustPressed);
        player.AddInputEventDelegate(ButtonReleased, UpdateLoopType.Update, InputActionEventType.ButtonJustReleased);
    }

    void Update()
    {
        
    }

    private void ButtonPressed(InputActionEventData data)
    {
        input_buffer.Insert(0, new ButtonBuffer(game_controller.current_game_frame,data.actionName,true));
    }

    private void ButtonReleased(InputActionEventData data)
    {
        input_buffer.Insert(0, new ButtonBuffer(game_controller.current_game_frame, data.actionName, false));
    }

    public bool CheckBuffer(string input)
    {
        return CheckBuffer(input, PlayerPrefs.GetInt("buffer_window", 12), true);
    }

    public bool CheckBuffer(string input, int buffer_window, bool pressed)
    {
        foreach (ButtonBuffer bufferedInput in input_buffer)
        {
            //If we've fallen off our distance value, quit the for loop.
            if (bufferedInput.frame < game_controller.current_game_frame - buffer_window)
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
            if (bufferedInput.frame < game_controller.current_game_frame - buffer_window)
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
            if (bufferedInput.frame < game_controller.current_game_frame - buffer_window)
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
            if (bufferedInput.frame < game_controller.current_game_frame - buffer_window)
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
            if (GetIntVar("facing") == 1) direction = "Right";
            if (GetIntVar("facing") == -1) direction = "Left";
        }

        if (direction == "Backward")
        {
            if (GetIntVar("facing") == 1) direction = "Left";
            if (GetIntVar("facing") == -1) direction = "Right";
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