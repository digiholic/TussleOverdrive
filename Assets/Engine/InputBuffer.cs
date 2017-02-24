using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputBuffer : MonoBehaviour {
    public int playerNum = 0;

    private List<InputEvent> inputBuffer = new List<InputEvent>();
    private GameController game_controller;
    
    private float last_horizontal = 0.0f;
    private float last_vertical = 0.0f;

    public Dictionary<InputType, float> ControllerState = new Dictionary<InputType, float>()
    {
        {InputType.Attack, 0.0f },
        {InputType.Special, 0.0f },
        {InputType.Jump, 0.0f },
        {InputType.Shield, 0.0f },
        {InputType.Left, 0.0f },
        {InputType.Right, 0.0f },
        {InputType.Up, 0.0f },
        {InputType.Down, 0.0f }
    };

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
        {
            inputBuffer.Insert(0, new InputEvent(InputType.Attack, 1.0f, game_controller.current_game_frame));
            ControllerState[InputType.Attack] = 1.0f;
        }   
        if (Input.GetButtonDown(playerNum + "_Special"))
        {
            inputBuffer.Insert(0, new InputEvent(InputType.Special, 1.0f, game_controller.current_game_frame));
            ControllerState[InputType.Special] = 1.0f;
        }
        if (Input.GetButtonDown(playerNum + "_Jump"))
        {
            inputBuffer.Insert(0, new InputEvent(InputType.Jump, 1.0f, game_controller.current_game_frame));
            ControllerState[InputType.Jump] = 1.0f;
        }
        if (Input.GetButtonDown(playerNum + "_Shield"))
        {
            inputBuffer.Insert(0, new InputEvent(InputType.Shield, 1.0f, game_controller.current_game_frame));
            ControllerState[InputType.Shield] = 1.0f;
        }
            
        //BUTTON RELEASE
        if (Input.GetButtonUp(playerNum + "_Attack"))
        {
            inputBuffer.Insert(0, new InputEvent(InputType.Attack, 0.0f, game_controller.current_game_frame));
            ControllerState[InputType.Attack] = 0.0f;
        }
            
        if (Input.GetButtonUp(playerNum + "_Special"))
        {
            inputBuffer.Insert(0, new InputEvent(InputType.Special, 0.0f, game_controller.current_game_frame));
            ControllerState[InputType.Special] = 0.0f;
        }
            
        if (Input.GetButtonUp(playerNum + "_Jump"))
        {
            inputBuffer.Insert(0, new InputEvent(InputType.Jump, 0.0f, game_controller.current_game_frame));
            ControllerState[InputType.Jump] = 0.0f;
        }
            
        if (Input.GetButtonUp(playerNum + "_Shield"))
        {
            inputBuffer.Insert(0, new InputEvent(InputType.Shield, 0.0f, game_controller.current_game_frame));
            ControllerState[InputType.Shield] = 0.0f;
        }
            
        //HORIZONTAL AXIS MOTION
        float haxis = Input.GetAxisRaw(playerNum + "_Horizontal");
        float hdiff = Mathf.Abs(haxis - last_horizontal);
        if (haxis >= 0.0f) //If it's to the right or zero
        {
            ControllerState[InputType.Right] = haxis;
            ControllerState[InputType.Left] = 0.0f;
        } else
        {
            ControllerState[InputType.Left] = Mathf.Abs(haxis);
            ControllerState[InputType.Right] = 0.0f;
        }
        
        if (hdiff >= 0.5) //If it's big enough, that's a smash
        {
            if (haxis < 0.0f) //Left
            {
                inputBuffer.Insert(0,new InputEvent(InputType.LeftSmash, Mathf.Abs(haxis), game_controller.current_game_frame));
                inputBuffer.Insert(0,new InputEvent(InputType.RightSmash, 0.0f, game_controller.current_game_frame));
            }
            else
            {
                inputBuffer.Insert(0,new InputEvent(InputType.RightSmash, Mathf.Abs(haxis), game_controller.current_game_frame));
                inputBuffer.Insert(0, new InputEvent(InputType.LeftSmash, 0.0f, game_controller.current_game_frame));
            }
            last_horizontal = haxis;   
        }
        
        if (hdiff >= 0.1) //if it's smaller, it's a tilt
        {
            if (haxis < 0.0f) //Left
            {
                inputBuffer.Insert(0,new InputEvent(InputType.Left, Mathf.Abs(haxis), game_controller.current_game_frame));
                inputBuffer.Insert(0,new InputEvent(InputType.Right, 0.0f, game_controller.current_game_frame));
            }
            else
            {
                inputBuffer.Insert(0,new InputEvent(InputType.Right, Mathf.Abs(haxis), game_controller.current_game_frame));
                inputBuffer.Insert(0,new InputEvent(InputType.Left, 0.0f, game_controller.current_game_frame));
            }

            last_horizontal = haxis;
        }
        
        //VERTICAL AXIS MOTION
        float vaxis = Input.GetAxisRaw(playerNum + "_Vertical");
        float vdiff = Mathf.Abs(vaxis - last_vertical);
        if (vaxis >= 0.0f) //If it's to the right or zero
        {
            ControllerState[InputType.Up] = haxis;
            ControllerState[InputType.Down] = 0.0f;
        }
        else
        {
            ControllerState[InputType.Down] = Mathf.Abs(haxis);
            ControllerState[InputType.Up] = 0.0f;
        }
        
        if (vdiff >= 0.5) //If it's big enough, that's a smash
        {
            if (vaxis < 0.0f) //Left
            {
                inputBuffer.Insert(0,new InputEvent(InputType.DownSmash, Mathf.Abs(vaxis), game_controller.current_game_frame));
                inputBuffer.Insert(0,new InputEvent(InputType.UpSmash, 0.0f, game_controller.current_game_frame));
            }
            else
            {
                inputBuffer.Insert(0,new InputEvent(InputType.UpSmash, Mathf.Abs(vaxis), game_controller.current_game_frame));
                inputBuffer.Insert(0,new InputEvent(InputType.DownSmash, 0.0f, game_controller.current_game_frame));
            }
            last_vertical = vaxis;
        }
        if (vdiff >= 0.1) //if it's smaller, it's a tilt
        {
            if (vaxis < 0.0f) //Left
            {
                inputBuffer.Insert(0,new InputEvent(InputType.Down, Mathf.Abs(vaxis), game_controller.current_game_frame));
                inputBuffer.Insert(0,new InputEvent(InputType.Up, 0.0f, game_controller.current_game_frame));
            }
            else
            {
                inputBuffer.Insert(0,new InputEvent(InputType.Up, Mathf.Abs(vaxis), game_controller.current_game_frame));
                inputBuffer.Insert(0,new InputEvent(InputType.Down, 0.0f, game_controller.current_game_frame));
            }
            last_vertical = vaxis;
        }
    }

    public bool KeyBuffered(InputType input, int distance = 12, float threshold = 0.1f)
    {
        bool threshValue;
        foreach (InputEvent bufferedInput in inputBuffer)
            if (bufferedInput.frame < game_controller.current_game_frame - distance) //If we've fallen off our distance value, end the iteration
                break;
            else
            {
                if (threshold > 0.0f)
                    threshValue = (bufferedInput.value >= threshold);
                else
                    threshValue = (bufferedInput.value <= Mathf.Abs(threshold));

                if (bufferedInput.key == input && threshValue)
                    if (bufferedInput.active)
                        return bufferedInput.Consume();
            }
        return false;
    }

    public bool SequenceBuffered(List<KeyValuePair<InputType,float>> inputList, int distance = 12)
    {
        int index = 0;
        bool threshValue;
        foreach (InputEvent bufferedInput in inputBuffer)
            if (bufferedInput.frame < game_controller.current_game_frame - distance) //If we've fallen off our distance value
                break;
            else if (index >= inputList.Count)
                break;
            else
            {
                if (inputList[index].Value > 0.0f)
                    threshValue = (bufferedInput.value >= inputList[index].Value);
                else
                    threshValue = (bufferedInput.value <= Mathf.Abs(inputList[index].Value));
                if (bufferedInput.key == inputList[index].Key && threshValue)
                    if (bufferedInput.active)
                        index++;
            }

        if (index >= inputList.Count) //if we've gotten through the list
            return true;
        return false;
    }
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

public enum InputType
{
    Attack, Special, Jump, Shield,
    Left, Right, Up, Down,
    LeftSmash, RightSmash, UpSmash, DownSmash, Grab
}


public static class InputTypeUtil
{
    public static InputType GetForward(AbstractFighter actor)
    {
        if (actor.facing == 1) //facing right
            return InputType.Right;
        else
            return InputType.Left;
    }

    public static InputType GetBackward(AbstractFighter actor)
    {
        if (actor.facing == 1) //facing right
            return InputType.Left;
        else
            return InputType.Right;
    }

    public static InputType GetForwardSmash(AbstractFighter actor)
    {
        if (actor.facing == 1) //facing right
            return InputType.RightSmash;
        else
            return InputType.LeftSmash;
    }

    public static InputType GetBackwardSmash(AbstractFighter actor)
    {
        if (actor.facing == 1) //facing right
            return InputType.LeftSmash;
        else
            return InputType.RightSmash;
    }
}