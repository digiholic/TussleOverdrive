using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuInputManager : InputManager {
    public static MenuButtonNavigator selectedButton = null;

    void Update()
    {
        foreach (InputType input_type in System.Enum.GetValues(typeof(InputType)))
            if (GetKeyDown(input_type))
                selectedButton.SendMessage("OnKeyPressed", input_type);

        foreach (InputType input_type in System.Enum.GetValues(typeof(InputType)))
            if (GetKeyUp(input_type))
                selectedButton.SendMessage("OnKeyReleased", input_type);
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
