using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoxResizerHandle : MonoBehaviour
{
    public enum HandlePosition {
        TOPLEFT, TOP, TOPRIGHT,
        LEFT, CENTER, RIGHT,
        BOTTOMLEFT, BOTTOM, BOTTOMRIGHT
    }

    public HandlePosition position;

    private bool isBeingDragged = false;
    [SerializeField] private ViewerBoxDisplayer boxDisplayer;
    void OnEnable()
    {
        if (boxDisplayer == null) boxDisplayer = GetComponentInParent<ViewerBoxDisplayer>();
    }

    // Update is called once per frame
    void Update()
    {
        if (isBeingDragged){
            snapToCursor();
            if (position == HandlePosition.TOPLEFT || position == HandlePosition.LEFT || position == HandlePosition.BOTTOMLEFT){
                boxDisplayer.boxRect.xMin = transform.localPosition.x;
            }
            if (position == HandlePosition.TOPRIGHT || position == HandlePosition.RIGHT || position == HandlePosition.BOTTOMRIGHT){
                boxDisplayer.boxRect.xMax = transform.localPosition.x;
            }
            if (position == HandlePosition.TOPLEFT || position == HandlePosition.TOP || position == HandlePosition.TOPRIGHT){
                boxDisplayer.boxRect.yMax = transform.localPosition.y;
            }
            if (position == HandlePosition.BOTTOMLEFT || position == HandlePosition.BOTTOM || position == HandlePosition.BOTTOMRIGHT){
                boxDisplayer.boxRect.yMin = transform.localPosition.y;
            }
            if (position == HandlePosition.CENTER){
                boxDisplayer.boxRect.center = transform.localPosition;
            }
        }
        else {
            snapToPosition();
        }
    }

    public void OnHoverStart(){
        /* float scalVal = transform.localScale.x*hoverScale;
        Vector3 newScale = new Vector3(hoverScale,transform.localScale.y*hoverScale,transform.localScale.z);

        TweenScale.Begin(this,0.2f,newScale); */
    }

    public void OnHoverEnd(){
        
    }

    public void OnClicked()
    {
        isBeingDragged = true;
        boxDisplayer.isResizing = true;
    }

    public void OnClickReleased(){
        isBeingDragged = false;
        boxDisplayer.isResizing = false;
        boxDisplayer.setSubactionFromData();
    }

    private void snapToCursor(){
        Vector2 mousePos = LegacyEditorData.instance.MouseToPixelHandler.CurrentPixelDistance;
        transform.localPosition = new Vector3(mousePos.x,mousePos.y,transform.localPosition.z);
    }

    private void snapToPosition(){
        Vector2 boxCenter = boxDisplayer.boxRect.center;
        Vector2 boxSize = boxDisplayer.boxRect.size;
        Vector3 newPosition = new Vector3(boxCenter.x,boxCenter.y,0);
        float mySize = transform.localPosition.x; //X and Y should be equal so we only need to get one
        
        //REMEMBER: Up is Positive Y

        //If it's a "top" or "bottom" position, set the y component of the position
        if (position == HandlePosition.TOPLEFT || position == HandlePosition.TOP || position == HandlePosition.TOPRIGHT){
            newPosition.y = boxCenter.y + (boxSize.y / 2);
        }
        else if (position == HandlePosition.BOTTOMLEFT || position == HandlePosition.BOTTOM || position == HandlePosition.BOTTOMRIGHT){
            newPosition.y = boxCenter.y - (boxSize.y / 2);
        }

        //If it's a "left" or "right" position, set the x component of the position
        if (position == HandlePosition.TOPRIGHT || position == HandlePosition.RIGHT || position == HandlePosition.BOTTOMRIGHT){
            newPosition.x = boxCenter.x + (boxSize.x / 2);
        }
        else if (position == HandlePosition.TOPLEFT || position == HandlePosition.LEFT || position == HandlePosition.BOTTOMLEFT){
            newPosition.x = boxCenter.x - (boxSize.x / 2);
        }

        transform.localPosition = newPosition;
    }
}
