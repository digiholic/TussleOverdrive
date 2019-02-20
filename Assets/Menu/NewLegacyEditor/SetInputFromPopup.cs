using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SetInputFromPopup : MonoBehaviour
{
    [SerializeField]
    private UIInput input;
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
        //Doing a bit of dark voodoo magic to call this private method in a library without modifying the library
        //This will let us do everything the UIInput does as if we typed it in
        input.SendMessage("Append", selectedInput+'\n');
    }
}
