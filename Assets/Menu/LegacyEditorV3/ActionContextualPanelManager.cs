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
        newSubactionPanel.SetActive(newAction != null && PanelNavigator.PanelNavigators["ActionCategory"].currentIndex != 0);
    }

    void OnActionCategoryChanged(int categoryIndex)
    {
        newSubactionPanel.SetActive(categoryIndex != 0);
    }

    private void OnEnable()
    {
        LESelectedAction.OnActionSelected += OnActionSelected;
        PanelNavigator.GetPanelEvent("ActionCategory").AddListener(OnActionCategoryChanged);
    }

    private void OnDisable()
    {
        LESelectedAction.OnActionSelected -= OnActionSelected;
        PanelNavigator.GetPanelEvent("ActionCategory").RemoveListener(OnActionCategoryChanged);
    }
}
