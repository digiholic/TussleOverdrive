using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuInputManager : InputManager {
    public static MenuButtonNavigator selectedButton = null;

    void Update()
    {
        
    }

    /// <summary>
    /// Switch the currently Selected button to the given value
    /// </summary>
    /// <param name="newButton"></param>
    public static void Select(MenuButtonNavigator newButton)
    {

        if (selectedButton != null)
        {
            selectedButton.Deselect();
        }
        newButton.Select();
        selectedButton = newButton;
    }
}
