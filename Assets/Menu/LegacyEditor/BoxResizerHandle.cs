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
    void Awake()
    {
        if (boxDisplayer == null) boxDisplayer = GetComponentInParent<ViewerBoxDisplayer>();
    }

    // Update is called once per frame
    void Update()
    {
        if (!isBeingDragged){
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
        Debug.Log("I've been clicked!");
    }

    private void snapToPosition(){
        Vector2 boxCenter = boxDisplayer.center;
        Vector2 boxSize = boxDisplayer.size;
        Vector3 newPosition = new Vector3(boxCenter.x,boxCenter.y,0);
        float mySize = transform.localPosition.x; //X and Y should be equal so we only need to get one
        
        //REMEMBER: Up is Positive Y

        //If it's a "top" or "bottom" position, set the y component of the position
        if (position == HandlePosition.TOPLEFT || position == HandlePosition.TOP || position == HandlePosition.TOPRIGHT){
            newPosition.y = (boxSize.y / 2);
        }
        else if (position == HandlePosition.BOTTOMLEFT || position == HandlePosition.BOTTOM || position == HandlePosition.BOTTOMRIGHT){
            newPosition.y = -(boxSize.y / 2);
        }

        //If it's a "left" or "right" position, set the x component of the position
        if (position == HandlePosition.TOPRIGHT || position == HandlePosition.RIGHT || position == HandlePosition.BOTTOMRIGHT){
            newPosition.x = (boxSize.x / 2);
        }
        else if (position == HandlePosition.TOPLEFT || position == HandlePosition.LEFT || position == HandlePosition.BOTTOMLEFT){
            newPosition.x = -(boxSize.x / 2);
        }

        transform.localPosition = newPosition;
    }
}
