using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TooltipSubactionVarData : MonoBehaviour
{
    private SubactionVarDataPanel subactionVarDataPanel;

    private void Awake()
    {
        subactionVarDataPanel = GetComponentInParent<SubactionVarDataPanel>();
    }
    // Update is called once per frame
    void Update()
    {
        if (UICamera.hoveredObject == gameObject)
        {
            TooltipDisplay.DisplayTooltip(subactionVarDataPanel.varData.description,gameObject);
        }
    }
}
