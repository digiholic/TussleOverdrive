using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DynamicGridExpander : MonoBehaviour {
    private DynamicGridCell thisCell;
    private int lastKnownHeight;

    public int headerSize;
    public DynamicGridLayout internalData;

	// Use this for initialization
	void OnEnable () {
        thisCell = GetComponent<DynamicGridCell>();
        thisCell.owner = internalData;
        lastKnownHeight = headerSize + internalData.GetTotalSize();
    }

    // Update is called once per frame
    void Update () {
        if (headerSize + internalData.GetTotalSize() != lastKnownHeight)
        {
            lastKnownHeight = headerSize + internalData.GetTotalSize();
            Reposition();
        }
	}

    void Reposition()
    {
        thisCell.height = lastKnownHeight;
        internalData.Reposition();
    }
}
