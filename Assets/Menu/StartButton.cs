using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using Rewired;
using TMPro;

public class StartButton : MonoBehaviour {
    public string SceneOnPress = "TopMenu";

    private TextMeshProUGUI textComponent;

    private float alpha = 144.0f;
    private float alpha_rad = 0.0f;

    private Player system_player;

	// Use this for initialization
	void Start () {
        textComponent = GetComponent<TextMeshProUGUI>();
        system_player = ReInput.players.GetSystemPlayer();
	}
	
	// Update is called once per frame
	void Update () {
        alpha = 127 * Mathf.Sin(alpha_rad) + 128;
        alpha_rad += 0.05f;
        textComponent.color = new Color(0, 0, 0, alpha / 255.0f);
        if (system_player.GetButtonUp("Escape") || system_player.GetButtonUp("Quit"))
        {
            Application.Quit();
        }
        foreach(Player player in ReInput.players.Players)
        {
            if (player.GetAnyButtonDown())
            {
                SceneManager.LoadScene(SceneOnPress, LoadSceneMode.Single);
            }
        }
	}
}
