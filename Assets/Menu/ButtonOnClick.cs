using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class ButtonOnClick : MonoBehaviour {
    private EventSystem events;

    public void Start()
    {
        events = GameObject.Find("EventSystem").GetComponent<EventSystem>();
    }

    public void Update()
    {
        if (events.currentSelectedGameObject == gameObject)
            if (Input.GetButtonUp("Submit"))
                OnClick();
    }

    public virtual void OnClick()
    {
        
    }
}
