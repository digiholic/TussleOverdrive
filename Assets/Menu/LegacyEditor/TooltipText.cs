using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TooltipText : MonoBehaviour
{
    [TextArea]
    public string tooltipText;

    private void Update()
    {
        if (UICamera.hoveredObject == gameObject)
        {
            TooltipDisplay.DisplayTooltip(tooltipText,gameObject);
        }
    }

    private void OnMouseEnter()
    {
        Debug.Log("MOUSE ENTER!");
    }
}
