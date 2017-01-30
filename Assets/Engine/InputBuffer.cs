using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputBuffer : MonoBehaviour {
    public int playerNum = 0;

    private Queue<KeyValuePair<int, List<KeyValuePair<string, float>>>> inputList; //Sweet christmas that type declaration
    private List<List<KeyValuePair<string, float>>> keyBuffer = new List<List<KeyValuePair<string, float>>>();

    private List<InputValue> inputBuffer = new List<InputValue>();

    private GameController game_controller;

    void Start()
    {
        game_controller = GameObject.Find("Controller").GetComponent<GameController>();
    }

    public void PushToQueue(int frame, List<KeyValuePair<string, float>> buttonInputList)
    {
        keyBuffer.Insert(0, buttonInputList);
        while (keyBuffer.Count > 12) //Unmagic this number
            keyBuffer.RemoveAt(keyBuffer.Count - 1);
    }

    /// <summary>
    /// Late Update will gather the inputs for the next frame
    /// </summary>
    void LateUpdate()
    {
        Debug.Log("LateUpdate");
        //BUTTON PRESS
        if (Input.GetButtonDown(playerNum + "_Attack"))
            inputBuffer.Add(new InputValue("attack", 1.0f, game_controller.current_game_frame));
        if (Input.GetButtonDown(playerNum + "_Special"))
            inputBuffer.Add(new InputValue("special", 1.0f, game_controller.current_game_frame));
        if (Input.GetButtonDown(playerNum + "_Jump"))
            inputBuffer.Add(new InputValue("jump", 1.0f, game_controller.current_game_frame));
        if (Input.GetButtonDown(playerNum + "_Shield"))
            inputBuffer.Add(new InputValue("shield", 1.0f, game_controller.current_game_frame));
        //BUTTON RELEASE
        if (Input.GetButtonUp(playerNum + "_Attack"))
            inputBuffer.Add(new InputValue("attack", 0.0f, game_controller.current_game_frame));
        if (Input.GetButtonUp(playerNum + "_Special"))
            inputBuffer.Add(new InputValue("special", 0.0f, game_controller.current_game_frame));
        if (Input.GetButtonUp(playerNum + "_Jump"))
            inputBuffer.Add(new InputValue("jump", 0.0f, game_controller.current_game_frame));
        if (Input.GetButtonUp(playerNum + "_Shield"))
            inputBuffer.Add(new InputValue("shield", 0.0f, game_controller.current_game_frame));
    }
}

[System.Serializable]
public class InputValue
{
    public string command;
    public float value;
    public int frame;
    public bool active;

    public InputValue(string _command, float _value, int _frame)
    {
        command = _command;
        value = _value;
        frame = _frame;
        active = true;
        Debug.Log(_command);
    }
}