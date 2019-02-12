using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TooltipSubaction : MonoBehaviour
{
    private SubactionCard subactionCard;

    private void Awake()
    {
        subactionCard = GetComponent<SubactionCard>();
    }
    // Update is called once per frame
    void Update()
    {
        if (UICamera.hoveredObject == gameObject)
        {
            TooltipDisplay.DisplayTooltip(subactionCard.subaction.description,gameObject);
        }
    }
}
