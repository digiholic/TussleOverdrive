using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TooltipDisplay : MonoBehaviour
{
    private static TooltipDisplay instance;
    private GameObject lastObjectHovered;
    private UILabel text;
    private float startingXPos;

    [SerializeField]
    private float scrollWidth = 1860;
    [SerializeField]
    private float timeToStartScrolling = 2;
    [SerializeField]
    private float timeToResetScrolling = 3;

    private float hoveredTime = 0;
    private float textWidthInPixels;
    
    [SerializeField]
    private float scrollSpeed = 1;
    private void Awake()
    {
        instance = this;
        text = GetComponent<UILabel>();
        startingXPos = transform.localPosition.x;
    }

    private void Update()
    {
        textWidthInPixels = NGUIMath.CalculateAbsoluteWidgetBounds(transform).size.x / LegacyEditorData.instance.transform.localScale.x;
        
        if (UICamera.hoveredObject == null)
        {
            DisplayTooltip("",null);
        }

        if (textWidthInPixels > scrollWidth)
        {
            hoveredTime += Time.deltaTime;
            if (hoveredTime >= timeToStartScrolling)
            {
                float xPos = transform.localPosition.x - (scrollSpeed * Time.deltaTime);
                //Since xPos will start to be negative, we need to see if it's already fully on screen before we actually move it
                if ((xPos + textWidthInPixels) > scrollWidth)
                {
                    transform.localPosition = new Vector3(xPos, transform.localPosition.y, transform.localPosition.z);
                } else
                {
                    //If we hit the end, wait for a few seconds and then reset the scrolling
                    Invoke("resetTimeAndPosition", timeToResetScrolling);
                }
            }
        }
    }

    private void resetTimeAndPosition()
    {
        hoveredTime = 0;
        transform.localPosition = new Vector3(startingXPos, transform.localPosition.y, transform.localPosition.z);
        //If we change hovered object while waiting for a reset on one, we need to cancel it or we'll interrupt the scrolling on the new one
        CancelInvoke();
    }

    private void changeTooltipDisplay(string tooltip, GameObject callingObject)
    {
        if (lastObjectHovered != callingObject)
        {
            lastObjectHovered = callingObject;
            text.text = tooltip;
            resetTimeAndPosition();
        }
    }

    public static void DisplayTooltip(string tooltip,GameObject callingObject)
    {
        if (tooltip == null) tooltip = "";
        tooltip = tooltip.Replace("\n", " ").Replace("\r", " ");
        instance.changeTooltipDisplay(tooltip, callingObject);
    }
}
