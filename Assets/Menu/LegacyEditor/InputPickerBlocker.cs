using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputPickerBlocker : MonoBehaviour
{
    [SerializeField]
    private InputPickerPopup popup;

    void OnClick()
    {
        popup.Dispose();
    }
}
