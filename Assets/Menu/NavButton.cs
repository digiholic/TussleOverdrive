using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class NavButton : MonoBehaviour {
    public string parentMenu;
    
    private EventSystem events;
    
    public void Start()
    {
        events = GameObject.Find("EventSystem").GetComponent<EventSystem>();
    }

    public void Update()
    {
        if (events.currentSelectedGameObject == gameObject)
            if (Input.GetButtonUp("Cancel"))
                OnCancel();
    }

    public void OnCancel()
    {
        if (parentMenu != "")
            gameObject.SendMessageUpwards("SwitchToString", parentMenu);
    }
}
