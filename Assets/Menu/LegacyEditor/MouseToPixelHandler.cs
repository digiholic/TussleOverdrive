using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouseToPixelHandler : MonoBehaviour
{
    [SerializeField] private Camera viewerCamera;
    [SerializeField] private SpriteHandler sprite;
    [SerializeField] private UILabel spriteDistanceDisplay;

    public Vector3 spriteCenterPoint;
    public float pixelsPerUnit = 100.0f;

    [SerializeField] private Vector2 pixelDistance;
    public Vector2 CurrentPixelDistance{
        get { return pixelDistance; }
    }
    // Update is called once per frame
    void Update()
    {
        spriteCenterPoint = sprite.getCenterPoint();
        Vector3 worldPoint = getWorldPositionOfMouse();
        pixelDistance = getPixelDistance(worldPoint);
        spriteDistanceDisplay.text = string.Format("({0},{1})",Mathf.FloorToInt(pixelDistance.x),Mathf.FloorToInt(pixelDistance.y));
    }

    private Vector3 getWorldPositionOfMouse(){
        Vector3 mousePoint = Vector3.zero;
        mousePoint.x = Input.mousePosition.x;
        mousePoint.y = Input.mousePosition.y;
        mousePoint.z = transform.position.z - viewerCamera.transform.position.z;
        return viewerCamera.ScreenToWorldPoint(mousePoint);
    }

    private Vector2 getPixelDistance(Vector3 worldPoint){
        Vector3 distance = worldPoint - spriteCenterPoint;
        Vector2 pixelDistance = Vector2.zero;
        pixelDistance.x = distance.x * pixelsPerUnit;
        pixelDistance.y = distance.y * pixelsPerUnit;
        return pixelDistance;
    }
}
