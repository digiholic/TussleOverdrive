using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Rewired;

public class GamepadManager : MonoBehaviour {
    public int player_num = 0;
    public bool push_to_buffer = true;

    private Player player;
    [SerializeField]
    private List<InputEvent> inputBuffer = new List<InputEvent>();
    private BattleController game_controller;

    private float smash_threshold;

    // Use this for initialization
    void Start()
    {
        player = ReInput.players.GetPlayer(player_num);
        
        if (push_to_buffer) game_controller = BattleController.current_battle;
    }



    // Update is called once per frame
    void Update()
    {
        if (push_to_buffer)
        {
            int f = game_controller.current_game_frame;
            float h_axis = player.GetAxis("Horizontal");
            Debug.Log(h_axis);
            inputBuffer.Insert(0, new InputEvent("Horizontal", h_axis, f));
            if (player.GetAxisDelta("Horizontal") > smash_threshold)
                inputBuffer.Insert(0, new InputEvent("HorizontalSmash", 1.0f, f));
        }
    }
}
