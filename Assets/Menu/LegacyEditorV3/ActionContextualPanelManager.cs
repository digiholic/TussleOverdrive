using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActionContextualPanelManager : MonoBehaviour
{
    public GameObject noActionSelectedPanel;
    public GameObject newSubactionPanel;

    void OnActionSelected(DynamicAction newAction)
    {
        noActionSelectedPanel.SetActive(newAction == null);
        newSubactionPanel.SetActive(newAction != null);
    }

    private void OnEnable()
    {
        LESelectedAction.OnActionSelected += OnActionSelected;
    }

    private void OnDisable()
    {
        LESelectedAction.OnActionSelected -= OnActionSelected;
    }
}
