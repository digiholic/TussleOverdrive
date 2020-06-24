using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ActionCategoryDropdown : MonoBehaviour
{
    [SerializeField] private TMP_Dropdown dropdown;

    public void OnActionSelected(DynamicAction action)
    {
        dropdown.interactable = (action != null);
    }
}
