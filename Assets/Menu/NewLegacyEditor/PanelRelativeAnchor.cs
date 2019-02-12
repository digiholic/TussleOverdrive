using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PanelRelativeAnchor : MonoBehaviour
{
    public Transform anchorObject;
    public UIAnchor.Side anchorSide;
    //public Vector3 offset;
    public UIAnchor.Side thisSide;

    private Vector3 startingPosition;
    
    // Start is called before the first frame update
    void Start()
    {
        startingPosition = transform.localPosition;
    }

    // Update is called once per frame
    void Update()
    {
        //Horizontal anchoring
        float anchorXPos = anchorObject.localPosition.x;
        float anchorWidth = anchorObject.localScale.x;

        float myXPos = startingPosition.x;
        float myWidth = transform.localScale.x;

        float resultXPos = myXPos;

        //Add the position of the anchor object if we're basing it off of left or right
        if (anchorSide == UIAnchor.Side.Left) resultXPos += anchorXPos;
        else if (anchorSide == UIAnchor.Side.Right) resultXPos += anchorXPos + anchorWidth;
        //If we're positioning by the right side, we need to subtract out the width of our object
        if (thisSide == UIAnchor.Side.Right) resultXPos -= myWidth;
        
        Vector3 newPos = new Vector3(resultXPos,startingPosition.y,startingPosition.z);
        transform.position = newPos;
    }

    [System.Serializable]
    public struct RelativeAnchor
    {
        public bool useAnchor;
        public Transform anchorObject;
        public UIAnchor.Side anchorSide;
        public Vector3 offset;
        public UIAnchor.Side thisSide;
    }
}
