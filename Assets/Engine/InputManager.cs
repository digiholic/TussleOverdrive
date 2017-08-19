using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Rewired;

public class InputManager : MonoBehaviour {
    public int player_num = 0;
    public bool push_to_buffer = true;

    private List<InputEvent> inputBuffer = new List<InputEvent>();
    private BattleController game_controller;
    private Player player;

    // Use this for initialization
	void Start () {
        player = ReInput.players.GetPlayer(player_num);
        if (push_to_buffer)
        {
            game_controller = BattleController.current_battle;
            player.AddInputEventDelegate(ButtonPressed, UpdateLoopType.Update, InputActionEventType.ButtonJustPressed);
            player.AddInputEventDelegate(NegativeButtonPressed, UpdateLoopType.Update, InputActionEventType.NegativeButtonJustPressed);
            player.AddInputEventDelegate(ButtonReleased, UpdateLoopType.Update, InputActionEventType.ButtonJustReleased);
            player.AddInputEventDelegate(NegativeButtonReleased, UpdateLoopType.Update, InputActionEventType.NegativeButtonJustReleased);
        }
    }

    void ButtonPressed(InputActionEventData data)
    {
        string input_type = data.actionName;
        if (input_type == "Horizontal") input_type = "Right";
        if (input_type == "Vertical") input_type = "Up";
        inputBuffer.Insert(0, new InputEvent(input_type, 1.0f, game_controller.current_game_frame));
    }

    void NegativeButtonPressed(InputActionEventData data)
    {
        string input_type = data.actionName;
        if (input_type == "Horizontal") input_type = "Left";
        if (input_type == "Vertical") input_type = "Down";
        inputBuffer.Insert(0, new InputEvent(input_type, 1.0f, game_controller.current_game_frame));
    }

    void ButtonReleased(InputActionEventData data)
    {
        string input_type = data.actionName;
        if (input_type == "Horizontal") input_type = "Right";
        if (input_type == "Vertical") input_type = "Up";
        inputBuffer.Insert(0, new InputEvent(input_type, 0.0f, game_controller.current_game_frame));
    }

    void NegativeButtonReleased(InputActionEventData data)
    {
        string input_type = data.actionName;
        if (input_type == "Horizontal") input_type = "Left";
        if (input_type == "Vertical") input_type = "Down";
        inputBuffer.Insert(0, new InputEvent(input_type, 0.0f, game_controller.current_game_frame));
    }

    
    /// <summary>
    /// Checks the buffer for an input without consuming it.
    /// This will also check consumed inputs. If you want to ignore consumed
    /// inputs, use the CheckBufferForValid function instead.
    /// </summary>
    /// <param name="input">The InputType to check for</param>
    /// <returns></returns>
    public bool CheckBuffer(string input)
    {
        return CheckBuffer(input, PlayerPrefs.GetInt("buffer_window", 12), 1.0f);
    }

    /// <summary>
    /// Checks the buffer for an input without consuming it.
    /// This will also check consumed inputs. If you want to ignore consumed
    /// inputs, use the CheckBufferForValid function instead.
    /// </summary>
    /// <param name="input">The InputType to check for</param>
    /// <param name="buffer_window">The distance to check back in the buffer. Defaults to the buffer window in the preferences.</param>
    /// <param name="value">The value to look for. If 1.0f, will check for presses. If 0.0f, will check for releases.</param>
    /// <returns></returns>
    public bool CheckBuffer(string input, int buffer_window, float value)
    {
        foreach (InputEvent bufferedInput in inputBuffer)
        {
            //If we've fallen off our distance value, quit the for loop.
            if (bufferedInput.frame < game_controller.current_game_frame - buffer_window)
                break;
            else
            {
                //If the key and value match what we're looking for, return true
                if (bufferedInput.key == input && bufferedInput.value == value)
                    return true;
            }
        }
        //If we didn't find it, return false.
        return false;
    }

    /// <summary>
    /// Checks the buffer for an input without consuming it.
    /// This will not check consumed inputs. If you want to check consumed
    /// inputs, use the CheckBuffer function instead.
    /// </summary>
    /// <param name="input">The InputType to check for</param>
    /// <returns></returns>
    public bool CheckBufferForValid(string input)
    {
        return CheckBufferForValid(input, PlayerPrefs.GetInt("buffer_window", 12), 1.0f);
    }

    /// <summary>
    /// Checks the buffer for an input without consuming it.
    /// This will not check consumed inputs. If you want to check consumed
    /// inputs, use the CheckBuffer function instead.
    /// </summary>
    /// <param name="input">The InputType to check for</param>
    /// <param name="buffer_window">The distance to check back in the buffer. Defaults to the buffer window in the preferences.</param>
    /// <param name="value">The value to look for. If 1.0f, will check for presses. If 0.0f, will check for releases.</param>
    /// <returns></returns>
    public bool CheckBufferForValid(string input, int buffer_window, float value)
    {
        foreach (InputEvent bufferedInput in inputBuffer)
        {
            //If we've fallen off our distance value, quit the for loop.
            if (bufferedInput.frame < game_controller.current_game_frame - buffer_window)
                break;
            else
            {
                //If the key and value match what we're looking for, return it if active.
                if (bufferedInput.key == input && bufferedInput.value == value)
                    if (bufferedInput.active)
                        return true;
            }
        }
        //If we didn't find it, return false.
        return false;
    }

    /// <summary>
    /// Checks the buffer for an input and consumes it.
    /// </summary>
    /// <param name="input">The InputType to check for</param>
    /// <returns></returns>
    public bool KeyBuffered(string input)
    {
        return KeyBuffered(input, PlayerPrefs.GetInt("buffer_window", 12), 1.0f);
    }

    /// <summary>
    /// Checks the buffer for an input and consumes it.
    /// </summary>
    /// <param name="input">The InputType to check for</param>
    /// <param name="buffer_window">The distance to check back in the buffer. Defaults to the buffer window in the preferences.</param>
    /// <param name="value">The value to look for. If 1.0f, will check for presses. If 0.0f, will check for releases.</param>
    /// <returns></returns>
    public bool KeyBuffered(string input, int buffer_window, float value)
    {
        foreach (InputEvent bufferedInput in inputBuffer)
        {
            //If we've fallen off our distance value, quit the for loop.
            if (bufferedInput.frame < game_controller.current_game_frame - buffer_window)
                break;
            else
            {
                //If the key and value match what we're looking for, return it if active.
                if (bufferedInput.key == input && bufferedInput.value == value)
                    return bufferedInput.Consume();
            }
        }
        //If we didn't find it, return false.
        return false;
    }

    /// <summary>
    /// Checks the buffer for a double-tap of the given input.
    /// That means it checks for a press, then release, then press again within the buffer window.
    /// </summary>
    /// <param name="input">The InputType to check for a double press</param>
    /// <returns>true if the given input type has been pressed twice in the buffer window</returns>
    public bool CheckDoubleTap(string input)
    {
        return CheckDoubleTap(input, PlayerPrefs.GetInt("buffer_window", 12));
    }

    /// <summary>
    /// Checks the buffer for a double-tap of the given input.
    /// That means it checks for a press, then release, then press again within the buffer window.
    /// </summary>
    /// <param name="input">The InputType to check for a double press</param>
    /// <param name="buffer_window">The distance to check back in the buffer. Defaults to the buffer window in the preferences.</param>
    /// <returns>true if the given input type has been pressed twice in the buffer window</returns>
    public bool CheckDoubleTap(string input, int buffer_window)
    {
        //Index and ValuesToFind track what we're looking for.
        //If Index is 3 at the end of the run, we've found all three values
        //In order to test the double-tap, we need to find a press, release, then press again
        //This is technically searching in reverse order, but it's symmetrical so it doesn't matter.
        int index = 0;
        float[] valuesToFind = new float[] { 1.0f, 0.0f, 1.0f };

        foreach (InputEvent bufferedInput in inputBuffer)
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
                if (bufferedInput.key == input && bufferedInput.value == valuesToFind[index])
                    index++;
            }
        }
        //If we've hit three things in the valuesToFind list, return true. Otherwise, false
        return (index >= valuesToFind.Length);
    }

    /// <summary>
    /// Threads the "GetKey" call to Input with the given InputType.
    /// Since multiple keys can have the same InputType, we have to check through all of them
    /// and if any of them are true, return true.
    /// </summary>
    /// <param name="input">The InputType to check for</param>
    /// <returns>Whether any of the keys assigned to that InputType are pressed.</returns>
    public bool GetKey(string input)
    {
        if (input == "Left")
            return player.GetNegativeButton("Horizontal");
        else if (input == "Right")
            return player.GetButton("Horizontal");

        else if (input == "Up")
            return player.GetNegativeButton("Vertical");
        else if (input == "Down")
            return player.GetButton("Vertical");

        else
            return player.GetButton(input);
    }

    /// <summary>
    /// Threads the "GetKeyDown" call to Input with the given InputType.
    /// Since multiple keys can have the same InputType, we have to check through all of them
    /// and if any of them are true, return true.
    /// </summary>
    /// <param name="input">The InputType to check for</param>
    /// <returns>Whether any of the keys assigned to that InputType were just pressed.</returns>
    public bool GetKeyDown(string input)
    {
        if (input == "Left")
            return player.GetNegativeButtonDown("Horizontal");
        else if (input == "Right")
            return player.GetButtonDown("Horizontal");

        else if (input == "Up")
            return player.GetNegativeButtonDown("Vertical");
        else if (input == "Down")
            return player.GetButtonDown("Vertical");

        else
            return player.GetButtonDown(input);
    }

    /// <summary>
    /// Threads the "GetKeyUp" call to Input with the given InputType.
    /// Since multiple keys can have the same InputType, we have to check through all of them
    /// and if any of them are true, return true.
    /// </summary>
    /// <param name="input">The InputType to check for</param>
    /// <returns>Whether any of the keys assigned to that InputType were just pressed.</returns>
    public bool GetKeyUp(string input)
    {
        if (input == "Left")
            return player.GetNegativeButtonUp("Horizontal");
        else if (input == "Right")
            return player.GetButtonUp("Horizontal");

        else if (input == "Up")
            return player.GetNegativeButtonUp("Vertical");
        else if (input == "Down")
            return player.GetButtonUp("Vertical");

        else
            return player.GetButtonUp(input);
    }
}

/*
public enum InputType
{
    Attack,
    Special,
    Jump,
    Shield,
    Left,
    Right,
    Up,
    Down
}
*/


public static class InputTypeUtil
{
    public static string GetForward(BattleObject actor)
    {
        if (actor.GetIntVar("facing") == 1) //facing right
            return "Right";
        else
            return "Left";
    }

    public static string GetBackward(BattleObject actor)
    {
        if (actor.GetIntVar("facing") == 1) //facing right
            return "Left";
        else
            return "Right";
    }
}


[System.Serializable]
public class InputEvent
{
    public string key;
    public float value;
    public int frame;
    public bool active;

    public InputEvent(string _key, float _value, int _frame)
    {
        key = _key;
        value = _value;
        frame = _frame;
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