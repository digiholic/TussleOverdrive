using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SubactionVarDataVarPicker : MonoBehaviour
{
    [SerializeField]
    private SubactionVarDataInput input;
    private UIPopupList popup;

    private void Start()
    {
        popup = GetComponent<UIPopupList>();
    }

    private void OnModelChanged()
    {
        //Don't enable this listener until the fighter is ready.
        //This gives us some time until the vars have already loaded before we set it
        if (LegacyEditorData.instance.loadedFighterDirty)
        {
            popup.eventReceiver = gameObject;
        }
    }

    void OnSelectionChange(string selectedInput)
    {
        Debug.Log("=====VARPICKER CHOSEN=====");
        input.OnAction(selectedInput);
    }
}
