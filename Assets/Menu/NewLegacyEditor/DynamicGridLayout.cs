using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DynamicGridLayout : MonoBehaviour {
    private List<DynamicGridCell> cells = new List<DynamicGridCell>();
    private int offset;

    public bool refreshNow;

    void Start()
    {
        ClearData();
        foreach (DynamicGridCell cell in GetComponentsInChildren<DynamicGridCell>())
        {
            AddData(cell);
        }
        //InvokeRepeating("Reposition", 0.0f, 0.5f);
        Reposition();
    }

    void Update()
    {
        if (refreshNow)
        {
            Reposition();
            refreshNow = false;
        }
    }

    public void AddData(DynamicGridCell cell)
    { 
        cells.Add(cell);
        cell.owner = this;
    }

    public void ClearData()
    {
        cells.Clear();
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
