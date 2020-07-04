using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class PanelNavigator : MonoBehaviour
{
    public static Dictionary<string, PanelNavigator> PanelNavigators = new Dictionary<string, PanelNavigator>();
    
    
    public string navDictName;
    public List<GameObject> subPanels;
    public int currentIndex = 0;

    void Start()
    {
        foreach(GameObject panel in subPanels)
        {
            panel?.SetActive(false);
        }
        subPanels[currentIndex]?.SetActive(true);
        if (navDictName != null && navDictName.Length > 0)
        {
            PanelNavigators.Add(navDictName, this);
        }
    }

    public void changeIndex(int newIndex)
    {
        subPanels[currentIndex]?.SetActive(false);
        currentIndex = newIndex;
        subPanels[newIndex]?.SetActive(true);
        GetPanelEvent(navDictName)?.Invoke(newIndex);
    }

    public int getCurrentIndex() { return currentIndex; }
    public GameObject getCurrentPanel() { return subPanels[currentIndex]; }
    public static Dictionary<string, PanelEvent> PanelEvents = new Dictionary<string, PanelEvent>();
    public static PanelEvent GetPanelEvent(string panelName)
    {
        if (!PanelEvents.ContainsKey(panelName))
            PanelEvents.Add(panelName, new PanelEvent());
        return PanelEvents[panelName];
    }
}

public class PanelEvent : UnityEvent<int> { };
