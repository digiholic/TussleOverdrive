using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DynamicGridLayout : MonoBehaviour {
    private List<DynamicGridCell> cells = new List<DynamicGridCell>();


    void Start()
    {
        Debug.Log(SubactionFactory.GetSubaction("Subaction"));

        foreach (DynamicGridCell cell in GetComponentsInChildren<DynamicGridCell>())
        {
            cells.Add(cell);
            cell.owner = this;
        }
        Reposition();
    }

    public void Reposition()
    {
        int offset = 0;
        foreach (DynamicGridCell cell in cells)
        {
            cell.transform.localPosition = new Vector3(cell.transform.localPosition.x, offset, cell.transform.localPosition.z);
            offset -= cell.height; //Must be subtracted because it's down
        }
    }
}
