using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DynamicGridLayout : MonoBehaviour {
    private List<DynamicGridCell> cells = new List<DynamicGridCell>();
    private int offset;

    public bool RepositionNow; //Used to refresh in play mode only

    void Start()
    {
        foreach (DynamicGridCell cell in GetComponentsInChildren<DynamicGridCell>())
        {
            cells.Add(cell);
            cell.owner = this;
        }
        Reposition();
    }

    void Update()
    {
        if (RepositionNow)
        {
            Reposition();
            RepositionNow = false;
        }
    }

    public void Reposition()
    {
        offset = 0;
        foreach (DynamicGridCell cell in cells)
        {
            cell.transform.localPosition = new Vector3(cell.transform.localPosition.x, offset, cell.transform.localPosition.z);
            offset -= cell.height; //Must be subtracted because it's down
        }
    }

    public int GetTotalSize()
    {
        return -offset;
    }
}
