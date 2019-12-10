using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Rewired;

public class MenuButtonNavigator : MonoBehaviour {
    public static MenuButtonNavigator selectedButton = null;
    public static bool accept_inputs = true;

    public bool DefaultSelected = false;

    public MenuButtonNavigator Right;
    public MenuButtonNavigator Left;
    public MenuButtonNavigator Up;
    public MenuButtonNavigator Down;

    public FuncData ConfirmFunction;
    public FuncData CancelFunction;
    public FuncData RightFunction;
    public FuncData LeftFunction;
    public FuncData UpFunction;
    public FuncData DownFunction;
    public FuncData OnSelectFunction;

    public bool selected = false;
    private static MenuButtonNavigator Last;

    void Start()
    {
        if (DefaultSelected)
        {
            selectedButton = this;
            selected = true;
            if (OnSelectFunction.FuncName != "") SendMessage(OnSelectFunction.FuncName, OnSelectFunction.FuncArg);
        }
    }

    void OnKeyPressed(string input)
    {
        if (selectedButton == this)
        {
            switch (input)
            {
                case ("Right"):
                    if (RightFunction.FuncName != "") SendMessage(RightFunction.FuncName, RightFunction.FuncArg);
                    if (Right != null)
                        Right.Select();
                    break;
                case ("Left"):
                    if (LeftFunction.FuncName != "") SendMessage(LeftFunction.FuncName, LeftFunction.FuncArg);
                    if (Left != null)
                        Left.Select();
                    break;
                case ("Up"):
                    if (UpFunction.FuncName != "") SendMessage(UpFunction.FuncName, UpFunction.FuncArg);
                    if (Up != null)
                        Up.Select();
                    break;
                case ("Down"):
                    if (DownFunction.FuncName != "") SendMessage(DownFunction.FuncName, DownFunction.FuncArg);
                    if (Down != null)
                        Down.Select();
                    break;
                case ("Attack"):
                    if (ConfirmFunction.FuncName != "") SendMessage(ConfirmFunction.FuncName,ConfirmFunction.FuncArg);
                    break;
                case ("Special"):
                    if (CancelFunction.FuncName != "") SendMessage(CancelFunction.FuncName, CancelFunction.FuncArg);
                    break;
                default:
                    break;

            }
        }
    }

    public void Select()
    {
        selectedButton.Deselect();
        selected = true;
        if (!string.IsNullOrEmpty(OnSelectFunction.FuncName)) SendMessage(OnSelectFunction.FuncName, OnSelectFunction.FuncArg);
    }

    public void Deselect()
    {
        selected = false;
    }
    
    void Update()
    {
        if (selectedButton == this)
        {
            SendMessage("SetColor", MenuColorChanger.getColor(), SendMessageOptions.DontRequireReceiver);
            if (accept_inputs)
            {
                foreach (Player player in ReInput.players.AllPlayers)
                {
                    if (player.GetButtonDown("Horizontal"))
                    {
                        OnKeyPressed("Right");
                        break;
                    }
                    if (player.GetNegativeButtonDown("Horizontal"))
                    {
                        OnKeyPressed("Left");
                        break;
                    }
                    if (player.GetButtonDown("Vertical"))
                    {
                        OnKeyPressed("Up");
                        break;
                    }
                    if (player.GetNegativeButtonDown("Vertical"))
                    {
                        OnKeyPressed("Down");
                        break;
                    }
                    if (player.GetButtonDown("Attack"))
                    {
                        OnKeyPressed("Attack");
                        break;
                    }
                    if (player.GetButtonDown("Special"))
                    {
                        OnKeyPressed("Special");
                        break;
                    }
                }
            }
        }
        else
            SendMessage("SetColor", Color.white,SendMessageOptions.DontRequireReceiver);

        //We have to do this after the button check, or else we get occasional double-moves
        if (selected) selectedButton = this;
    }

    public void LoadScene(string sceneName)
    {
        SceneManager.LoadScene(sceneName, LoadSceneMode.Single);
    }

    public void LoadSceneAndClearMenu(string sceneName)
    {
        Destroy(MenuColorChanger.menu_color.gameObject);
        SceneManager.LoadSceneAsync(sceneName, LoadSceneMode.Single);
    }

    public void SaveSettings(string sceneName)
    {
        Settings.current_settings.SaveSettings();
        ReInput.userDataStore.Save();
        if (sceneName != null) LoadScene(sceneName);
    }

    public void QuitGame()
    {
        Application.Quit();
    }

    public void SetLast()
    {
        Last = selectedButton;
    }

    public void SelectLast()
    {
        Last.Select();
    }

    public void ShowSettingsPanel(string panelName)
    {
        SettingsPanelRig.SelectPanel(panelName);
    }

    public void SelectSettingsPanel(string panelName)
    {
        SettingsPanelRig.GetPanel(panelName);
    }

    public void ExitPanel()
    {
        SettingsPanel.active_panel.settings_header.Select();
    }
}

[System.Serializable]
public struct FuncData
{
    public string FuncName;
    public string FuncArg;
}
