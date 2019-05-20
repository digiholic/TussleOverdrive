using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// This class maintains links to all of the cardinal anchors, and will give their world coordinates,
/// as well as the world height and width, when queried. This is used for responsiveness to
/// resizing windows.
/// </summary>
public class AnchorPositions : MonoBehaviour
{
    [SerializeField]
    private Transform leftAnchor, rightAnchor, topAnchor, bottomAnchor;

    public float left { get
        {
            return leftAnchor.position.x / transform.localScale.x;
        }
    }

    public float right
    {
        get
        {
            return rightAnchor.position.x / transform.localScale.x;
        }
    }

    public float top
    {
        get
        {
            return topAnchor.position.y / transform.localScale.y;
        }
    }

    public float bottom
    {
        get
        {
            return bottomAnchor.position.y / transform.localScale.y;
        }
    }

    public float width
    {
        get
        {
            return right - left;
        }
    }

    public float height
    {
        get
        {
            return top - bottom;
        }
    }
}
