using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class MenuNavigator : MonoBehaviour {
    public NamedMenu[] menuList;
    
    public enum MenuState { Title, Main, Tussle, Settings, Modules }

    private GameObject currentMenu;
    private Dictionary<string, GameObject> menuDict;

    void Awake()
    {
        menuDict = new Dictionary<string, GameObject>();
        //Build the menu dict
        foreach (NamedMenu menu in menuList)
        {
            menuDict.Add(menu.name, menu.menu);
            menu.menu.SetActive(false);
        }

    }

    void Start()
    {
        //Enable Title Screen
        currentMenu = menuDict["Title"];
        currentMenu.SetActive(true);
        currentMenu.GetComponent<MenuManager>().ActivateMenu();
    }           
                        
	// Update is called     once per frame
	void Update () {

    }


    public void QuitGame()
    {
        Debug.Log("Quitting");
        Application.Quit();
    }

    public void SwitchToString(string menuName)
    {
        currentMenu.SendMessage("DeactivateMenu");
        currentMenu = menuDict[menuName];
        currentMenu.SetActive(true);
        currentMenu.SendMessage("ActivateMenu");
    }

    public GameObject GetByString(string menuName)
    {
        return menuDict[menuName];
    }
}

[System.Serializable]
public struct NamedMenu
{
    public string name;
    public GameObject menu;
}