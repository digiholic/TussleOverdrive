using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuilderControlPane : MonoBehaviour {
    private bool isHovered = false;
    public UICamera viewerCamera;
    public BattleObject targetObject;

    private bool middleClickHeld;
    private Vector2 middleClickLastPos;

    private bool rightClickHeld;
    private Vector2 rightClickLastPos;

    public float scrollFactor = 0.1f;

    public float dragThreshold = 1;
    public float xDragMul;
    public float yDragMul;

    public float orbitThreshold = 1;
    public float xOrbitMul;
    public float yOrbitMul;

    public float minZoomDist = 2;
    public float maxZoomDist = 25;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        isHovered = (UICamera.hoveredObject == gameObject);

        processRightClick();
        processMiddleClick();
	}

    void processMiddleClick()
    {
        if (isHovered && Input.GetMouseButtonDown(2))
        {
            middleClickLastPos = UICamera.lastEventPosition;
            middleClickHeld = true;
        }

        if (middleClickHeld && Input.GetMouseButtonUp(2))
        {
            middleClickHeld = false;
        }

        if (middleClickHeld)
        {
            Vector2 middleClickCurrentPos = UICamera.lastEventPosition;
            Vector2 diff = middleClickCurrentPos - middleClickLastPos;
            //We want to avoid jittering so we'll only move if the difference is above a threshold.
            if (diff.magnitude >= dragThreshold)
            {
                viewerCamera.transform.Translate(new Vector3(diff.x * xDragMul, diff.y * yDragMul, 0));
                middleClickLastPos = middleClickCurrentPos;
            }
        }
    }

    void processRightClick()
    {
        if (isHovered && Input.GetMouseButtonDown(1))
        {
            rightClickLastPos = UICamera.lastEventPosition;
            rightClickHeld = true;
        }

        if (rightClickHeld && Input.GetMouseButtonUp(1))
        {
            rightClickHeld = false;
        }

        if (rightClickHeld)
        {
            Vector2 rightClickCurrentPos = UICamera.lastEventPosition;
            Vector2 diff = rightClickCurrentPos - rightClickLastPos;
            //We want to avoid jittering so we'll only move if the difference is above a threshold.
            if (diff.magnitude >= orbitThreshold)
            {
                //viewerCamera.transform.Translate(new Vector3(diff.x * xDragMul, diff.y * yDragMul, 0));
                viewerCamera.transform.RotateAround(targetObject.transform.position, Vector3.up, xOrbitMul * diff.x);
                viewerCamera.transform.RotateAround(targetObject.transform.position, Vector3.left, yOrbitMul * diff.y);
                //viewerCamera.transform.LookAt(targetObject.transform);
                rightClickLastPos = rightClickCurrentPos;
            }
        }
    }

    void OnScroll(float delta)
    {
        Vector3 oldZoom = viewerCamera.transform.localPosition;
        viewerCamera.transform.Translate(new Vector3(0, 0, delta * scrollFactor));

        //If we make the zoom and it's too far or too close, undo the zoom
        Vector3 distanceToFighter = viewerCamera.transform.localPosition - targetObject.transform.localPosition;
        Debug.Log(distanceToFighter);
        if (distanceToFighter.magnitude < minZoomDist  || distanceToFighter.magnitude > maxZoomDist)
        {
            viewerCamera.transform.localPosition = oldZoom;
        }
    }

    public void recenterCamera()
    {
        viewerCamera.transform.localPosition = new Vector3(0, 0, -10);
        viewerCamera.transform.localRotation = Quaternion.identity;
    }
}
