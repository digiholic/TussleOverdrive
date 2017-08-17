using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputManager : MonoBehaviour {
    public int player_num = 0;
    public bool push_to_buffer = true;

    private Dictionary<InputType, List<KeyCode>> input_map = new Dictionary<InputType, List<KeyCode>>();

    private List<InputEvent> inputBuffer = new List<InputEvent>();
    private BattleController game_controller;

    // Use this for initialization
	void Start () {
        if (push_to_buffer) game_controller = BattleController.current_battle;
        LoadAllKeys();
    }
    
    /// <summary>
    /// Loads all the InputTypes into the internal input_map.
    /// Called at the start, and whenever those keys change.
    /// </summary>
    public void LoadAllKeys()
    {
        LoadFromPrefs(InputType.Attack, "control_attack_" + player_num, "Z");
        LoadFromPrefs(InputType.Special, "control_special_" + player_num, "X");
        LoadFromPrefs(InputType.Jump, "control_jump_" + player_num, "C");
        LoadFromPrefs(InputType.Shield, "control_shield_" + player_num, "A");

        LoadFromPrefs(InputType.Left, "control_left_" + player_num, "LeftArrow");
        LoadFromPrefs(InputType.Right, "control_right_" + player_num, "RightArrow");
        LoadFromPrefs(InputType.Up, "control_up_" + player_num, "UpArrow");
        LoadFromPrefs(InputType.Down, "control_down_" + player_num, "DownArrow");

        //Forward and Backward are "virtual" keys, they are resolved by fighters into left or right.
        //InputManager never deals with them.
        input_map[InputType.Forward] = new List<KeyCode>();
        input_map[InputType.Backward] = new List<KeyCode>();

    }

    /// <summary>
    /// Loads the inputs from preferences for the given key. Sets the input_map internally,
    /// then returns the list of codes.
    /// </summary>
    /// <param name="input">InputType to load the keys for</param>
    /// <param name="name">The string name of the key in preferences</param>
    /// <param name="default_key">The default key in case the preference isn't set</param>
    private List<KeyCode> LoadFromPrefs(InputType input, string name, string default_key)
    {
        string inputString = PlayerPrefs.GetString(name, default_key);

        List<KeyCode> inputCodes = new List<KeyCode>();
        foreach (string code in inputString.Split(','))
            inputCodes.Add((KeyCode)System.Enum.Parse(typeof(KeyCode), code));
        if (input_map.ContainsKey(input))
            input_map[input] = inputCodes;
        else
            input_map.Add(input, inputCodes);
        return inputCodes;
    }



    // Update is called once per frame
    void Update () {
        if (push_to_buffer)
        {
            //Insert the pressed events
            foreach (InputType input_type in System.Enum.GetValues(typeof(InputType)))
                if (GetKeyDown(input_type))
                    inputBuffer.Insert(0, new InputEvent(input_type, 1.0f, game_controller.current_game_frame));

            //Insert release events
            foreach (InputType input_type in System.Enum.GetValues(typeof(InputType)))
                if (GetKeyUp(input_type))
                    inputBuffer.Insert(0, new InputEvent(input_type, 0.0f, game_controller.current_game_frame));
        }
    }

    /// <summary>
    /// Checks the buffer for an input without consuming it.
    /// This will also check consumed inputs. If you want to ignore consumed
    /// inputs, use the CheckBufferForValid function instead.
    /// </summary>
    /// <param name="input">The InputType to check for</param>
    /// <returns></returns>
    public bool CheckBuffer(InputType input)
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
    public bool CheckBuffer(InputType input, int buffer_window, float value)
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
    public bool CheckBufferForValid(InputType input)
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
    public bool CheckBufferForValid(InputType input, int buffer_window, float value)
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
    public bool KeyBuffered(InputType input)
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
    public bool KeyBuffered(InputType input, int buffer_window, float value)
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
    public bool CheckDoubleTap(InputType input)
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
    public bool CheckDoubleTap(InputType input, int buffer_window)
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
    public bool GetKey(InputType input)
    {
        bool retValue = false;
        foreach (KeyCode code in input_map[input])
        {
            retValue = retValue || Input.GetKey(code);
        }
        return retValue;
    }

    /// <summary>
    /// Threads the "GetKeyDown" call to Input with the given InputType.
    /// Since multiple keys can have the same InputType, we have to check through all of them
    /// and if any of them are true, return true.
    /// </summary>
    /// <param name="input">The InputType to check for</param>
    /// <returns>Whether any of the keys assigned to that InputType were just pressed.</returns>
    public bool GetKeyDown(InputType input)
    {
        bool retValue = false;
        foreach (KeyCode code in input_map[input])
        {
            retValue = retValue || Input.GetKeyDown(code);
        }
        return retValue;
    }

    /// <summary>
    /// Threads the "GetKeyUp" call to Input with the given InputType.
    /// Since multiple keys can have the same InputType, we have to check through all of them
    /// and if any of them are true, return true.
    /// </summary>
    /// <param name="input">The InputType to check for</param>
    /// <returns>Whether any of the keys assigned to that InputType were just pressed.</returns>
    public bool GetKeyUp(InputType input)
    {
        bool retValue = false;
        foreach (KeyCode code in input_map[input])
        {
            retValue = retValue || Input.GetKeyUp(code);
        }
        return retValue;
    }


}

public enum InputType
{
    Attack, Special, Jump, Shield,
    Left, Right, Up, Down,
    Forward, Backward
}



public static class InputTypeUtil
{
    public static InputType GetForward(BattleObject actor)
    {
        if (actor.GetIntVar("facing") == 1) //facing right
            return InputType.Right;
        else
            return InputType.Left;
    }

    public static InputType GetBackward(BattleObject actor)
    {
        if (actor.GetIntVar("facing") == 1) //facing right
            return InputType.Left;
        else
            return InputType.Right;
    }
    /*
    public static InputType GetForwardSmash(BattleObject actor)
    {
        if (actor.GetIntVar("facing") == 1) //facing right
            return InputType.RightSmash;
        else
            return InputType.LeftSmash;
    }

    public static InputType GetBackwardSmash(BattleObject actor)
    {
        if (actor.GetIntVar("facing") == 1) //facing right
            return InputType.LeftSmash;
        else
            return InputType.RightSmash;
    }
    */
}

[System.Serializable]
public class InputEvent
{
    public InputType key;
    public float value;
    public int frame;
    public bool active;

    public InputEvent(InputType _key, float _value, int _frame)
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

public enum ControlType
{
    KEYBOARD,
    GAMEPAD
}