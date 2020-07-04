using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActionContextualPanelManager : MonoBehaviour
{
    public GameObject noActionSelectedPanel;
    public GameObject newSubactionPanel;
    public GameObject defaultEditSubactionPanel;

    void OnActionSelected(DynamicAction newAction)
    {
        noActionSelectedPanel.SetActive(newAction == null);
        newSubactionPanel.SetActive(newAction != null && PanelNavigator.PanelNavigators["ActionCategory"].currentIndex != 0);
    }

    void OnActionCategoryChanged(int categoryIndex)
    {
        newSubactionPanel.SetActive(categoryIndex != 0);
    }

    void OnSubactionSelected(SubactionData data)
    {
        defaultEditSubactionPanel.SetActive(data != null);
        newSubactionPanel.SetActive(data == null);
    }

    private void OnEnable()
    {
        LESelectedAction.OnActionSelected += OnActionSelected;
        PanelNavigator.GetPanelEvent("ActionCategory").AddListener(OnActionCategoryChanged);
        LESelectedSubaction.OnSubactionSelected += OnSubactionSelected;
    }

    private void OnDisable()
    {
        LESelectedAction.OnActionSelected -= OnActionSelected;
        PanelNavigator.GetPanelEvent("ActionCategory").RemoveListener(OnActionCategoryChanged);
        LESelectedSubaction.OnSubactionSelected -= OnSubactionSelected;
    }
}
