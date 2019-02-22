using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(UILabel))]
public class InputPickerData : MonoBehaviour
{
    public InputPickerPopup popup;
    
    private UILabel label;

    void Awake()
    {
        label = GetComponent<UILabel>();
    }

    void OnClick()
    {
        popup.OnInputSelected(label.text);
        popup.Dispose();
    }

    public void SetLabel(string text)
    {
        label.text = text;
    }
}
