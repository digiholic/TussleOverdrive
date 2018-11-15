using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuilderControlPane : MonoBehaviour {
    private bool isHovered = false;
    public UICamera viewerCamera;

    private bool middleClickHeld;
    private Vector2 middleClickLastPos;

    public float dragThreshold = 1;
    public float xDragMul;
    public float yDragMul;
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        isHovered = (UICamera.hoveredObject == gameObject);

        if (isHovered && Input.GetMouseButtonDown(2))
        {
            middleClickLastPos = UICamera.lastTouchPosition;
            middleClickHeld = true;

        }

        if (middleClickHeld && Input.GetMouseButtonUp(2))
            middleClickHeld = false;

        if (middleClickHeld)
        {
            Vector2 middleClickCurrentPos = UICamera.lastTouchPosition;
            Vector2 diff = middleClickCurrentPos - middleClickLastPos;
            //We want to avoid jittering so we'll only move if the difference is above a threshold.
            if (diff.magnitude >= dragThreshold)
            {
                viewerCamera.transform.Translate(new Vector3(diff.x * xDragMul, diff.y * yDragMul, 0));
                middleClickLastPos = middleClickCurrentPos;
            }
        }
	}

    void OnScroll(float delta)
    {
        viewerCamera.transform.Translate(new Vector3(0, 0, delta*0.1f));
    }
}
