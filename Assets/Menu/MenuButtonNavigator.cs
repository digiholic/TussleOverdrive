using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuButtonNavigator : MonoBehaviour {
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
            MenuInputManager.Select(this);
        }
    }

    void OnKeyPressed(string input)
    {
        if (MenuInputManager.selectedButton == this)
        {
            switch (input)
            {
                case ("Right"):
                    if (Right != null)
                        MenuInputManager.Select(Right);
                    else if (RightFunction.FuncName != "") SendMessage(RightFunction.FuncName, RightFunction.FuncArg);
                    break;
                case ("Left"):
                    if (Left != null)
                        MenuInputManager.Select(Left);
                    else if (LeftFunction.FuncName != "") SendMessage(LeftFunction.FuncName, LeftFunction.FuncArg);
                    break;
                case ("Up"):
                    if (Up != null)
                        MenuInputManager.Select(Up);
                    else if (UpFunction.FuncName != "") SendMessage(UpFunction.FuncName, UpFunction.FuncArg);
                    break;
                case ("Down"):
                    if (Down != null)
                        MenuInputManager.Select(Down);
                    else if (DownFunction.FuncName != "") SendMessage(DownFunction.FuncName, DownFunction.FuncArg);
                    break;
                case ("Attack"):
                    if (ConfirmFunction.FuncName != "") SendMessage(ConfirmFunction.FuncName,ConfirmFunction.FuncArg);
                    break;
                case ("Special"):
                    if (CancelFunction.FuncName != "") SendMessage(CancelFunction.FuncName, CancelFunction.FuncArg);
                    break;

            }
        }
    }

    void OnKeyReleased(string input)
    {

    }

    public void Select()
    {
        selected = true;
    }

    public void Deselect()
    {
        selected = false;
    }
    
    void Update()
    {
        if (selected)
            SendMessage("SetColor", MenuColorChanger.menu_color.getColor());
        else
            SendMessage("SetColor", Color.white);
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
