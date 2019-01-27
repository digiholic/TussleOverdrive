using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TooltipSubactionCreateButton : MonoBehaviour
{
    private SubactionCreateButton button;

    // Start is called before the first frame update
    void Awake()
    {
        button = GetComponent<SubactionCreateButton>();
    }

    // Update is called once per frame
    void Update()
    {
        if (UICamera.hoveredObject == gameObject)
        {
            TooltipDisplay.DisplayTooltip(button.subData.Description);
        }
    }
}
