using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class MenuManager : MonoBehaviour {
    public GameObject DefaultSelected;

    private GameObject LastSelected;
    private EventSystem events;

    void Awake()
    {
        LastSelected = DefaultSelected;
        events = GameObject.Find("EventSystem").GetComponent<EventSystem>();
    }

    public void ActivateMenu()
    {
        if (LastSelected == null)
            LastSelected = DefaultSelected;

        events.SetSelectedGameObject(LastSelected);
        gameObject.SetActive(true);
    }

    public void DeactivateMenu()
    {
        LastSelected = events.currentSelectedGameObject;
        gameObject.SetActive(false);
    }
}
