using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShowPanelIfDropdown : LegacyEditorWidget {
    public string leftDropdownCond;
    public string rightDropdownCond;

    public bool debug;

    void OnDropdownChanged(string s)
    {
        OnModelChanged();
    }

    void OnModelChanged()
    {
        bool leftVal = true;
        bool rightVal = true;
        //If the dropdown conditional is empty, we just keep the condition true
        if (!leftDropdownCond.Equals(""))
        {
            if (editor.leftDropdown.Equals(leftDropdownCond))
            {
                if (debug) Debug.Log("Left Value Valid");
                leftVal = true;
            } else
            {
                if (debug) Debug.Log("Left Value Invalid");
                leftVal = false;
            }
        }

        //If the dropdown conditional is empty, we just keep the condition true
        if (!rightDropdownCond.Equals(""))
        {
            if (editor.rightDropdown.Equals(rightDropdownCond))
            {
                if (debug) Debug.Log("Right Value Valid");
                rightVal = true;
            }
            else
            {
                if (debug) Debug.Log("Right Value Invalid");
                rightVal = false;
            }
        }

        //If both conditionals are true
        if (leftVal && rightVal)
        {
            LegacyEditorData.Unbanish(gameObject);
        } else
        {
            LegacyEditorData.Banish(gameObject);
        }
    }

    public override void RegisterListeners()
    {
        editor.LeftDropdownChangedEvent += OnDropdownChanged;
        editor.RightDropdownChangedEvent += OnDropdownChanged;
    }

    public override void UnregisterListeners()
    {
        editor.LeftDropdownChangedEvent -= OnDropdownChanged;
        editor.RightDropdownChangedEvent -= OnDropdownChanged;
    }
}
