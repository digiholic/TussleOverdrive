using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SubactionVarDataVarPicker : MonoBehaviour
{
    [SerializeField]
    private InputPickerPopup popupBox;

    void OnPress()
    {
        NGUITools.SetActive(popupBox.gameObject,true);
        popupBox.refreshGridAndLabels();
    }
}
