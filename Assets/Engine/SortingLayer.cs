using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SortingLayer : MonoBehaviour {
    
    public string sortingLayerName;        // The name of the sorting layer .
    public int sortingOrder;            //The sorting order

    void Start()
    {
        Renderer r = GetComponent<Renderer>();
        // Set the sorting layer and order.
        r.sortingLayerName = sortingLayerName;
        r.sortingOrder = sortingOrder;
    }
}
