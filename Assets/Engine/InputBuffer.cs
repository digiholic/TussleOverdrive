using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputBuffer : MonoBehaviour {
    public int playerNum = 0;

    private List<InputValue> inputBuffer = new List<InputValue>();
    private GameController game_controller;

    private float last_horizontal = 0.0f;
    private float last_vertical = 0.0f;
    void Start()
    {
        game_controller = GameObject.Find("Controller").GetComponent<GameController>();
    }

    
    /// <summary>
    /// Late Update will gather the inputs for the next frame
    /// </summary>
    void LateUpdate()
    {
        //BUTTON PRESS
        if (Input.GetButtonDown(playerNum + "_Attack"))
            inputBuffer.Add(new InputValue(InputType.Attack, 1.0f, game_controller.current_game_frame));
        if (Input.GetButtonDown(playerNum + "_Special"))
            inputBuffer.Add(new InputValue(InputType.Special, 1.0f, game_controller.current_game_frame));
        if (Input.GetButtonDown(playerNum + "_Jump"))
            inputBuffer.Add(new InputValue(InputType.Jump, 1.0f, game_controller.current_game_frame));
        if (Input.GetButtonDown(playerNum + "_Shield"))
            inputBuffer.Add(new InputValue(InputType.Shield, 1.0f, game_controller.current_game_frame));
        //BUTTON RELEASE
        if (Input.GetButtonUp(playerNum + "_Attack"))
            inputBuffer.Add(new InputValue(InputType.Attack, 0.0f, game_controller.current_game_frame));
        if (Input.GetButtonUp(playerNum + "_Special"))
            inputBuffer.Add(new InputValue(InputType.Special, 0.0f, game_controller.current_game_frame));
        if (Input.GetButtonUp(playerNum + "_Jump"))
            inputBuffer.Add(new InputValue(InputType.Jump, 0.0f, game_controller.current_game_frame));
        if (Input.GetButtonUp(playerNum + "_Shield"))
            inputBuffer.Add(new InputValue(InputType.Shield, 0.0f, game_controller.current_game_frame));
        //HORIZONTAL AXIS MOTION
        float haxis = Input.GetAxis(playerNum + "_Horizontal");
        float hdiff = Mathf.Abs(haxis - last_horizontal);
        if (hdiff >= 0.5) //If it's big enough, that's a smash
        {
            if (haxis < 0.0f) //Left
            {
                inputBuffer.Add(new InputValue(InputType.LeftSmash, Mathf.Abs(haxis), game_controller.current_game_frame));
                inputBuffer.Add(new InputValue(InputType.RightSmash, 0.0f, game_controller.current_game_frame));
            }
            else
            {
                inputBuffer.Add(new InputValue(InputType.RightSmash, Mathf.Abs(haxis), game_controller.current_game_frame));
                inputBuffer.Add(new InputValue(InputType.LeftSmash, 0.0f, game_controller.current_game_frame));
            }
            last_horizontal = haxis;   
        }
        else if (hdiff >= 0.1) //if it's smaller, it's a tilt
        {
            if (haxis < 0.0f) //Left
            {
                inputBuffer.Add(new InputValue(InputType.Left, Mathf.Abs(haxis), game_controller.current_game_frame));
                inputBuffer.Add(new InputValue(InputType.Right, 0.0f, game_controller.current_game_frame));
            }
            else
            {
                inputBuffer.Add(new InputValue(InputType.Right, Mathf.Abs(haxis), game_controller.current_game_frame));
                inputBuffer.Add(new InputValue(InputType.Left, 0.0f, game_controller.current_game_frame));
            }

            last_horizontal = haxis;
        }
        
        //VERTICAL AXIS MOTION
        float vaxis = Input.GetAxis(playerNum + "_Vertical");
        float vdiff = Mathf.Abs(vaxis - last_vertical);
        if (vdiff >= 0.5) //If it's big enough, that's a smash
        {
            if (vaxis < 0.0f) //Left
            {
                inputBuffer.Add(new InputValue(InputType.DownSmash, Mathf.Abs(vaxis), game_controller.current_game_frame));
                inputBuffer.Add(new InputValue(InputType.UpSmash, 0.0f, game_controller.current_game_frame));
            }
            else
            {
                inputBuffer.Add(new InputValue(InputType.UpSmash, Mathf.Abs(vaxis), game_controller.current_game_frame));
                inputBuffer.Add(new InputValue(InputType.DownSmash, 0.0f, game_controller.current_game_frame));
            }
            last_vertical = vaxis;
        }
        else if (vdiff >= 0.1) //if it's smaller, it's a tilt
        {
            if (vaxis < 0.0f) //Left
            {
                inputBuffer.Add(new InputValue(InputType.Down, Mathf.Abs(vaxis), game_controller.current_game_frame));
                inputBuffer.Add(new InputValue(InputType.Up, 0.0f, game_controller.current_game_frame));
            }
            else
            {
                inputBuffer.Add(new InputValue(InputType.Up, Mathf.Abs(vaxis), game_controller.current_game_frame));
                inputBuffer.Add(new InputValue(InputType.Down, 0.0f, game_controller.current_game_frame));
            }
            last_vertical = vaxis;
        }
    }

    public bool KeyBuffered(InputType input, int distance = 12, float threshold = 0.1f)
    {
        foreach (InputValue bufferedInput in inputBuffer)
        {
            if (bufferedInput.frame >= game_controller.current_game_frame - distance)
                {
                    if (bufferedInput.key == input && bufferedInput.value >= threshold)
                    {
                        return true;
                    }
                }
        }
        return false;
    }
}


[System.Serializable]
public class InputValue
{
    public InputType key;
    public float value;
    public int frame;
    public bool active;

    public InputValue(InputType _key, float _value, int _frame)
    {
        key = _key;
        value = _value;
        frame = _frame;
        active = true;
    }
}

public enum InputType
{
    Attack, Special, Jump, Shield,
    Left, Right, Up, Down,
    LeftSmash, RightSmash, UpSmash, DownSmash, Grab
}