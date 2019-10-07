using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class FileMenu : MonoBehaviour
{
    private UIPopupList popupList;
    
    //A list of all the menu options and the function to call when they're clicked. Set in the Unity editor.
    public List<TopMenuOption> options;
    private void Start()
    {
        popupList = GetComponent<UIPopupList>();
        foreach(TopMenuOption opt in options){
            popupList.items.Add(opt.menuOptionText);
        }
    }

    public void OnSelectionChange()
    {
        string selected = popupList.value;
        foreach(TopMenuOption opt in options){
            if (opt.menuOptionText == selected){
                opt.functionToCall.Invoke();
            }
        }
    }
}

[System.Serializable]
public class TopMenuOption{
    public string menuOptionText;
    public UnityEvent functionToCall;
}
