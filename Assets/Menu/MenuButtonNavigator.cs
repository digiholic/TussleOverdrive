using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Rewired;

public class MenuButtonNavigator : MonoBehaviour {
    public static MenuButtonNavigator selectedButton = null;

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

    private bool selected = false;
    
    void Awake()
    {
        if (DefaultSelected)
        {
            selectedButton = this;
            selected = true;
        }
    }

    void OnKeyPressed(string input)
    {
        if (selectedButton == this)
        {
            switch (input)
            {
                case ("Right"):
                    if (Right != null)
                        Right.Select();
                    else if (RightFunction.FuncName != "") SendMessage(RightFunction.FuncName, RightFunction.FuncArg);
                    break;
                case ("Left"):
                    if (Left != null)
                        Left.Select();
                    else if (LeftFunction.FuncName != "") SendMessage(LeftFunction.FuncName, LeftFunction.FuncArg);
                    break;
                case ("Up"):
                    if (Up != null)
                        Up.Select();
                    else if (UpFunction.FuncName != "") SendMessage(UpFunction.FuncName, UpFunction.FuncArg);
                    break;
                case ("Down"):
                    if (Down != null)
                        Down.Select();
                    else if (DownFunction.FuncName != "") SendMessage(DownFunction.FuncName, DownFunction.FuncArg);
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
    }

    public void Deselect()
    {
        selected = false;
    }
    
    void Update()
    {
        if (selectedButton == this)
        {
            SendMessage("SetColor", MenuColorChanger.menu_color.getColor());
            foreach (Player player in ReInput.players.Players)
            {
                if (player.GetButtonDown("Horizontal")) OnKeyPressed("Right");
                if (player.GetNegativeButtonDown("Horizontal")) OnKeyPressed("Left");
                if (player.GetButtonDown("Vertical")) OnKeyPressed("Up");
                if (player.GetNegativeButtonDown("Vertical")) OnKeyPressed("Down");
                if (player.GetButtonDown("Attack")) OnKeyPressed("Attack");
                if (player.GetButtonDown("Special")) OnKeyPressed("Special");
            }
        }
        else
            SendMessage("SetColor", Color.white);

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

    public void QuitGame()
    {
        Application.Quit();
    }
}

[System.Serializable]
public struct FuncData
{
    public string FuncName;
    public string FuncArg;
}
