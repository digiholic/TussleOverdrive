using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PopulatePopup : MonoBehaviour
{
    [SerializeField]
    protected InputPickerPopup popup;

    protected void PopulateList(List<string> textList)
    {
        popup.getItems().Clear();
        foreach (string text in textList)
        {
            popup.getItems().Add(text);
        }
        popup.generateItems();
    }
}
