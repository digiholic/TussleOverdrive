using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PanelNavigator : MonoBehaviour
{
    public List<GameObject> subPanels;
    public int currentIndex = 0;

    void Start()
    {
        foreach(GameObject panel in subPanels)
        {
            panel.SetActive(false);
        }
        subPanels[currentIndex].SetActive(true);
    }

    public void changeIndex(int newIndex)
    {
        subPanels[currentIndex].SetActive(false);
        currentIndex = newIndex;
        subPanels[newIndex].SetActive(true);
    }
}
