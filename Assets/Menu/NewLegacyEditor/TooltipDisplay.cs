using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TooltipDisplay : MonoBehaviour
{
    private static TooltipDisplay instance;
    private UILabel text;

    private void Awake()
    {
        instance = this;
        text = GetComponent<UILabel>();
    }

    private void Update()
    {
        if (UICamera.hoveredObject == null)
        {
            DisplayTooltip("");
        }
    }

    public static void DisplayTooltip(string tooltip)
    {
        tooltip = tooltip.Replace("\n", " ").Replace("\r", " ");
        instance.text.text = tooltip;
    }
}
